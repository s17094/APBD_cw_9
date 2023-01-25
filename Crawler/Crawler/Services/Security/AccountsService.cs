using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

using Crawler.Context;
using Crawler.Models.DTO;
using Crawler.Models.Security;
using Microsoft.IdentityModel.Tokens;

namespace Crawler.Services.Security;

public class AccountsService : IAccountsService
{

    private readonly IConfiguration _configuration;
    private readonly MainDbContext _context;

    public AccountsService(IConfiguration configuration, MainDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    public void Register(RegisterRequest registerRequest)
    {
        var hashedPasswordAndSalt = GetHashedPasswordAndSalt(registerRequest.Password);

        var user = new User
        {
            Email = registerRequest.Email,
            Login = registerRequest.Login,
            Password = hashedPasswordAndSalt.Item1,
            Salt = hashedPasswordAndSalt.Item2,
            RefreshToken = GenerateRefreshToken(),
            RefreshTokenExp = DateTime.Now.AddDays(1)
        };

        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public Tuple<string, string> Login(LoginRequest loginRequest)
    {
        var user = _context.Users
            .FirstOrDefault(u => u.Login == loginRequest.Login);

        if (user == null)
        {
            return null;
        }

        var passwordHashFromDb = user.Password;
        var curHashedPassword = GetHashedPasswordWithSalt(loginRequest.Password, user.Salt);

        if (passwordHashFromDb != curHashedPassword)
        {
            return null;
        }

        var userClaim = new[]
        {
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, "user"),
            new Claim(ClaimTypes.Role, "admin"),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Host"],
            audience: _configuration["Host"],
            claims: userClaim,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );

        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExp = DateTime.Now.AddDays(1);
        _context.SaveChanges();

        return new Tuple<string, string>(new JwtSecurityTokenHandler().WriteToken(token), user.RefreshToken);
    }

    public Tuple<string, string> Refresh(string token, RefreshTokenRequest refreshToken)
    {
        var user = _context.Users
            .FirstOrDefault(u => u.RefreshToken == refreshToken.RefreshToken);
        if (user == null)
        {
            throw new SecurityTokenException("Invalid refresh token");
        }

        if (user.RefreshTokenExp < DateTime.Now)
        {
            throw new SecurityTokenException("Refresh token expired");
        }

        var login = GetUserIdFromAccessToken(token.Replace("Bearer ", ""), _configuration["SecretKey"]);

        var userClaim = new[]
        {
            new Claim(ClaimTypes.Name, login),
            new Claim(ClaimTypes.Role, "user"),
            new Claim(ClaimTypes.Role, "admin")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            issuer: _configuration["Host"],
            audience: _configuration["Host"],
            claims: userClaim,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );

        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExp = DateTime.Now.AddDays(1);
        _context.SaveChanges();

        return new Tuple<string, string>(new JwtSecurityTokenHandler().WriteToken(jwtToken), user.RefreshToken);
    }

    private Tuple<string, string> GetHashedPasswordAndSalt(string password)
    {
        var salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        var saltBase64 = Convert.ToBase64String(salt);

        return new Tuple<string, string>(hashed, saltBase64);
    }

    private string GetHashedPasswordWithSalt(string password, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);

        var currentHashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: saltBytes,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return currentHashedPassword;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    private string GetUserIdFromAccessToken(string accessToken, string secret)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateActor = true,
            ClockSkew = TimeSpan.FromMinutes(2),
            ValidIssuer = _configuration["Host"],
            ValidAudience = _configuration["Host"],
            ValidateLifetime = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
        {
            throw new SecurityTokenException("Invalid token!");
        }

        var userId = principal.FindFirst(ClaimTypes.Name)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            throw new SecurityTokenException($"Missing claim: {ClaimTypes.Name}!");
        }

        return userId;

    }

}