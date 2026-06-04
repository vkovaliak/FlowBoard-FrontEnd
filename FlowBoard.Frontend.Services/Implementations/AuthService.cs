using FlowBoard.Frontend.Domain.DTOs.Auth;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Http;

namespace FlowBoard.Frontend.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IAuthApi _authApi;
    private readonly ITokenService _tokenService;

    public AuthService(IAuthApi authApi, ITokenService tokenService)
    {
        _authApi = authApi;
        _tokenService = tokenService;
    }

    public async Task<bool> LoginAsync(UserLoginDto dto)
    {
        var response = await _authApi.LoginAsync(dto);

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            await _tokenService.SaveTokensAsync(response.Content);
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
            return true;
        }
        return false;
    }
}