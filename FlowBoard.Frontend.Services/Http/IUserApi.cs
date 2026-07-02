using FlowBoard.Frontend.Domain.DTOs.Users;
using Refit;

namespace FlowBoard.Frontend.Services.Http;

public interface IUserApi
{
    [Multipart]
    [Put("/api/users/avatar")]
    Task<ApiResponse<AvatarResponseDto>> UpdateAvatarAsync(StreamPart file);

    [Put("/api/users/username")]
    Task<ApiResponse<bool>> UpdateUserNameAsync(
        [Body] UpdateUserNameDto dto);
    
    [Get("/api/users/me")]
    Task<ApiResponse<UserDto>> GetMeAsync();

    [Delete("/api/users/avatar")]
    Task<ApiResponse<bool>> DeleteAvatarAsync();

    [Put("/api/users/change-password")]
    Task<ApiResponse<bool>> ChangePasswordAsync(ChangePasswordDto dto);
}