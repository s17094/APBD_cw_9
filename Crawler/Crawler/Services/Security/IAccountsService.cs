using Crawler.Models.DTO;

namespace Crawler.Services.Security;

public interface IAccountsService
{
    void Register(RegisterRequest registerRequest);

    Tuple<string, string>? Login(LoginRequest loginRequest);

    Tuple<string, string> Refresh(string token, RefreshTokenRequest refreshToken);
}