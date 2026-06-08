using Blazored.LocalStorage;
using FlowBoard.Frontend.Domain.Constants;
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
        await _localStorage.SetItemAsync(TokenStorageKeys.AccessToken, tokenDto.AccessToken);
        await _localStorage.SetItemAsync(TokenStorageKeys.RefreshToken, tokenDto.RefreshToken);
        
        await _localStorage.SetItemAsync(TokenStorageKeys.AccessTokenExpires, tokenDto.AccessTokenExpirationTime);
        await _localStorage.SetItemAsync(TokenStorageKeys.RefreshTokenExpires, tokenDto.RefreshTokenExpirationTime);

        await _localStorage.SetItemAsync(TokenStorageKeys.AccessTokenCreatedAt, DateTime.UtcNow);
    }

    public async Task<string?> GetAccessTokenAsync()
    {
        return await _localStorage.GetItemAsync<string>(TokenStorageKeys.AccessToken);
    }

    public async Task<string?> GetRefreshTokenAsync()
    {
        return await _localStorage.GetItemAsync<string>(TokenStorageKeys.RefreshToken);
    }

    public async Task<DateTime?> GetAccessTokenExpirationAsync()
    {
        return await _localStorage.GetItemAsync<DateTime>(TokenStorageKeys.AccessTokenExpires);
    }

    public async Task<DateTime?> GetRefreshTokenExpirationAsync()
    {
        return await _localStorage.GetItemAsync<DateTime>(TokenStorageKeys.RefreshTokenExpires);
    }

    public async Task<DateTime?> GetAccessTokenCreatedAtAsync()
    {
        return await _localStorage.GetItemAsync<DateTime>(TokenStorageKeys.AccessTokenCreatedAt);
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
        var createdAt = await GetAccessTokenCreatedAtAsync();

        if (expiresAt is null || createdAt is null)
            return;

        var totalLifetime = expiresAt.Value - createdAt.Value;

        var now = DateTime.UtcNow;

        var halfPoint = createdAt.Value + TimeSpan.FromTicks(totalLifetime.Ticks / 2);

        if (now >= halfPoint)
        {
            await RefreshTokenAsync();
        }
    }
}