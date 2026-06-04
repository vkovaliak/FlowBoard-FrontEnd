using Blazored.LocalStorage;
using FlowBoard.Frontend.Domain.DTOs.Auth;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Http;

namespace FlowBoard.Frontend.Services.Implementations;

public class TokenService : ITokenService
{
    private readonly ILocalStorageService _localStorage;
    private readonly IAuthApi _authApi;

    public TokenService(ILocalStorageService localStorage, IAuthApi authApi)
    {
        _localStorage = localStorage;
        _authApi = authApi;
    }

    public async Task SaveTokensAsync(TokenDto tokenDto)
    {
        await _localStorage.SetItemAsync("access_token", tokenDto.AccessToken);
        await _localStorage.SetItemAsync("refresh_token", tokenDto.RefreshToken);
        
        await _localStorage.SetItemAsync("access_token_expires", tokenDto.AccessTokenExpirationTime);
        await _localStorage.SetItemAsync("refresh_token_expires", tokenDto.RefreshTokenExpirationTime);
    }

    public async Task<string?> GetAccessTokenAsync()
    {
        return await _localStorage.GetItemAsync<string>("access_token");
    }

    public async Task<string?> GetRefreshTokenAsync()
    {
        return await _localStorage.GetItemAsync<string>("refresh_token");
    }

    public async Task<DateTime?> GetAccessTokenExpirationAsync()
    {
        return await _localStorage.GetItemAsync<DateTime>("access_token_expires");
    }

    public async Task<DateTime?> GetRefreshTokenExpirationAsync()
    {
        return await _localStorage.GetItemAsync<DateTime>("refresh_token_expires");
    }

    public async Task<bool> RefreshTokenAsync()
    {
        var refreshToken = await GetRefreshTokenAsync();

        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return false;
        }

        var response = await _authApi.RefreshTokenAsync(
            new RefreshTokenDto(refreshToken));

        if (!response.IsSuccessStatusCode ||
            response.Content is null)
        {
            return false;
        }

        await SaveTokensAsync(response.Content);

        return true;
    }

    public async Task RefreshIfNeededAsync()
    {
        var expiresAt = await GetAccessTokenExpirationAsync();

        if (expiresAt is null)
            return;

        var now = DateTime.UtcNow;

        var totalLifetime = expiresAt.Value - now;

        var halfPoint = expiresAt.Value - TimeSpan.FromTicks(totalLifetime.Ticks / 2);

        if (now >= halfPoint)
        {
            await RefreshTokenAsync();
        }
    }
}