using FlowBoard.Frontend.Domain.DTOs.Auth;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface IAuthService
{
    Task<bool> RegisterAsync(UserRegisterDto dto);
    Task<bool> LoginAsync(UserLoginDto dto);
}