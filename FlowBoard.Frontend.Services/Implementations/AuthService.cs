using FlowBoard.Frontend.Domain.DTOs.Auth;
using FlowBoard.Frontend.Domain.Models.Common;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Helpers;
using FlowBoard.Frontend.Services.Http;
using FlowBoard.Frontend.Services.Providers;

namespace FlowBoard.Frontend.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IAuthApi _authApi;
    private readonly ITokenService _tokenService;
    private readonly CustomAuthStateProvider _authStateProvider;
    private readonly IMicrosoftAuthService _microsoftAuthService;

    public AuthService(IAuthApi authApi, 
        ITokenService tokenService,
        CustomAuthStateProvider authStateProvider,
        IMicrosoftAuthService microsoftAuthService)
    {
        _authApi = authApi;
        _tokenService = tokenService;
        _authStateProvider = authStateProvider;
        _microsoftAuthService = microsoftAuthService;
    }

    public async Task<OperationResult> LoginAsync(UserLoginDto dto)
    {
        var response = await _authApi.LoginAsync(dto);

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            await _tokenService.SaveTokensAsync(response.Content);
            _authStateProvider.NotifyUserAuthentication(response.Content.AccessToken);
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> RegisterAsync(UserRegisterDto dto)
    {
        var response = await _authApi.RegisterAsync(dto);

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            await _tokenService.SaveTokensAsync(response.Content);
            _authStateProvider.NotifyUserAuthentication(response.Content.AccessToken);
            return OperationResult.Ok();
        }
        
        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> LogoutAsync()
    {
        var refreshToken = await _tokenService.GetRefreshTokenAsync();
        if (refreshToken == null)
        {
           return OperationResult.Fail("Refresh token not found.");
        }
        var request = new RefreshTokenDto(refreshToken);
        var response = await _authApi.LogoutAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return OperationResult.Fail(response.GetErrorMessage());
        }

        await _tokenService.RemoveTokensAsync();
        _authStateProvider.NotifyUserLogout();
        
        return OperationResult.Ok();
    }

    public async Task<OperationResult> LoginWithMicrosoftAsync()
    {
        var idToken = await _microsoftAuthService.GetIdTokenAsync();

        if (string.IsNullOrEmpty(idToken))
        {
            return OperationResult.Fail("Failed to retrieve Microsoft ID token.");
        }

        var dto = new ExternalTokenDto(idToken);
        
        var response = await _authApi.ExternalMicrosoftAsync(dto);

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            await _tokenService.SaveTokensAsync(response.Content);

            _authStateProvider.NotifyUserAuthentication(
                response.Content.AccessToken);

            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }
}