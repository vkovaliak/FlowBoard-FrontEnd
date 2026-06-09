using FlowBoard.Frontend.Domain.DTOs.Auth;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface ITokenService
{
    Task SaveTokensAsync(TokenDto tokenDto);

    Task<string?> GetAccessTokenAsync();
    Task<string?> GetRefreshTokenAsync();

    Task<DateTime?> GetAccessTokenExpirationAsync();
    Task<DateTime?> GetRefreshTokenExpirationAsync();

    Task<bool> RefreshTokenAsync();
    Task RefreshIfNeededAsync();

    Task RemoveTokensAsync();
}