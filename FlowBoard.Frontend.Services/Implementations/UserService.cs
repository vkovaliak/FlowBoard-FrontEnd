using FlowBoard.Frontend.Domain.DTOs.Users;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Http;
using Refit;

namespace FlowBoard.Frontend.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserApi _userApi;

    public UserService(IUserApi userApi)
    {
        _userApi = userApi;
    }

    public async Task<string?> UpdateAvatarAsync(Stream fileStream, string fileName)
    {
        var streamPart = new StreamPart(fileStream, fileName);
        var response = await _userApi.UpdateAvatarAsync(streamPart);

        return response.IsSuccessStatusCode && response.Content is not null
            ? response.Content.AvatarUrl
            : null;
    }

    public async Task<bool> UpdateUserNameAsync(string userName)
    {
        var dto = new UpdateUserNameDto(userName);
        var response = await _userApi.UpdateUserNameAsync(dto);

        return response.IsSuccessStatusCode
            && response.Content != false;
    }

    public async Task<UserDto?> GetMeAsync()
    {
        var response = await _userApi.GetMeAsync();

        return response.IsSuccessStatusCode
            ? response.Content
            : null;
    }

    public async Task<bool> DeleteAvatarAsync()
    {
        var response = await _userApi.DeleteAvatarAsync();

        return response.IsSuccessStatusCode
            && response.Content != false;
    }
}