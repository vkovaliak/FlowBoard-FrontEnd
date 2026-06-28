using FlowBoard.Frontend.Domain.DTOs.Auth;
using FlowBoard.Frontend.Domain.Models.Common;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface IAuthService
{
    Task<OperationResult> RegisterAsync(UserRegisterDto dto);
    Task<OperationResult> LoginAsync(UserLoginDto dto);
    Task<OperationResult> LogoutAsync();
    Task<OperationResult> LoginWithMicrosoftAsync();
}