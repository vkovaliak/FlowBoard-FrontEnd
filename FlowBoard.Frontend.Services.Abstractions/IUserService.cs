using FlowBoard.Frontend.Domain.DTOs.Users;
using FlowBoard.Frontend.Domain.Models.Common;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface IUserService
{
    Task<OperationResult<string?>> UpdateAvatarAsync(Stream fileStream, string fileName);
    Task<OperationResult> UpdateUserNameAsync(string userName);
    Task<UserDto?> GetMeAsync();
    Task<OperationResult> DeleteAvatarAsync(); 
    Task<OperationResult> ChangePasswordAsync(ChangePasswordDto dto);
}