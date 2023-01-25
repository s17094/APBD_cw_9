using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;

using Crawler.Models.DTO;
using Crawler.Services.Security;

namespace Crawler.Controllers.Security;

[Route("api/auth")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IAccountsService _accountsService;

    public AccountsController(IAccountsService accountsService)
    {
        _accountsService = accountsService;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest registerRequest)
    {
        _accountsService.Register(registerRequest);
        return Ok();
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest loginRequest)
    {
        var result = _accountsService.Login(loginRequest);

        if (result == null)
        {
            return Unauthorized();
        }
        
        return Ok(new
        {
            accessToken = result.Item1,
            refreshToken = result.Item2
        });
    }

    [HttpPost("refresh")]
    public IActionResult Refresh([FromHeader(Name = "Authorization")] string token, RefreshTokenRequest refreshToken)
    {
       var result =  _accountsService.Refresh(token, refreshToken);

       return Ok(new
       {
            accessToken = result.Item1,
            refreshToken = result.Item2
       });
    }

}