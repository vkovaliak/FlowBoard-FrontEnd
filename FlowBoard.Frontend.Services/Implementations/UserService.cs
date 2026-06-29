using FlowBoard.Frontend.Domain.DTOs.Users;
using FlowBoard.Frontend.Domain.Models.Common;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Helpers;
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

    public async Task<OperationResult<string?>> UpdateAvatarAsync(Stream fileStream, string fileName)
    {
        var streamPart = new StreamPart(fileStream, fileName);
        var response = await _userApi.UpdateAvatarAsync(streamPart);

        if (response.IsSuccessStatusCode && response.Content is not null)
        {
            return OperationResult<string?>.Ok(response.Content.AvatarUrl);
        }

        return OperationResult<string?>.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> UpdateUserNameAsync(string userName)
    {
        var dto = new UpdateUserNameDto(userName);
        var response = await _userApi.UpdateUserNameAsync(dto);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<UserDto?> GetMeAsync()
    {
        var response = await _userApi.GetMeAsync();

        return response.IsSuccessStatusCode
            ? response.Content
            : null;
    }

    public async Task<OperationResult> DeleteAvatarAsync()
    {
        var response = await _userApi.DeleteAvatarAsync();

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }
}