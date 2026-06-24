using FlowBoard.Frontend.Domain.DTOs.Auth;
using FlowBoard.Frontend.Services.Abstractions;
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

    public async Task<bool> LoginAsync(UserLoginDto dto)
    {
        var response = await _authApi.LoginAsync(dto);

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            await _tokenService.SaveTokensAsync(response.Content);
            _authStateProvider.NotifyUserAuthentication(response.Content.AccessToken);
            return true;
        }

        return false;
    }

    public async Task<bool> RegisterAsync(UserRegisterDto dto)
    {
        var response = await _authApi.RegisterAsync(dto);

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            await _tokenService.SaveTokensAsync(response.Content);
            _authStateProvider.NotifyUserAuthentication(response.Content.AccessToken);
            return true;
        }
        
        return false;
    }

    public async Task<bool> LogoutAsync()
    {
        var refreshToken = await _tokenService.GetRefreshTokenAsync();
        if(refreshToken == null)
        {
            return false;
        }
        var request = new RefreshTokenDto(refreshToken);
        await _authApi.LogoutAsync(request);

        await _tokenService.RemoveTokensAsync();
        _authStateProvider.NotifyUserLogout();
        
        return true;
    }

    public async Task<bool> LoginWithMicrosoftAsync()
    {
        var idToken = await _microsoftAuthService.GetIdTokenAsync();

        if (string.IsNullOrEmpty(idToken))
        {
            return false; 
        }

        var dto = new ExternalTokenDto(idToken);
        
        var response = await _authApi.ExternalMicrosoftAsync(dto);

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            await _tokenService.SaveTokensAsync(response.Content);

            _authStateProvider.NotifyUserAuthentication(
                response.Content.AccessToken);

            return true;
        }

        return false;
    }
}