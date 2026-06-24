using FlowBoard.Frontend.Domain.DTOs.Users;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface IUserService
{
    Task<string?> UpdateAvatarAsync(Stream fileStream, string fileName);
    Task<bool> UpdateUserNameAsync(string userName);
    Task<UserDto?> GetMeAsync();
    Task<bool> DeleteAvatarAsync(); 
}