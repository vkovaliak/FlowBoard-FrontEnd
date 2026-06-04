using FlowBoard.Frontend.Domain.DTOs.Auth;
using Refit;

namespace FlowBoard.Frontend.Services.Http;

public interface IAuthApi
{
    [Post("/api/auth/login")]
    Task<ApiResponse<TokenDto>> LoginAsync([Body] UserLoginDto dto);
    
    [Post("/api/auth/register")]
    Task<ApiResponse<TokenDto>> RegisterAsync([Body] UserRegisterDto dto);

    [Post("/api/auth/refresh")]
    Task<ApiResponse<TokenDto>> RefreshTokenAsync([Body] RefreshTokenDto request);
}