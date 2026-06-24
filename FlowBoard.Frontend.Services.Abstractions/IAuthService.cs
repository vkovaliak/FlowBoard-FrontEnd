using FlowBoard.Frontend.Domain.DTOs.Auth;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface IAuthService
{
    Task<bool> RegisterAsync(UserRegisterDto dto);
    Task<bool> LoginAsync(UserLoginDto dto);
    Task<bool> LogoutAsync();
    Task<bool> LoginWithMicrosoftAsync();
}