using FlowBoard.Frontend.Domain.DTOs.Users;
using FlowBoard.Frontend.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Pages;

public partial class Account
{
    [Inject] private IUserService UserService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    private UserDto? _user;
    private string _userNameInput = string.Empty;
    private bool _loading = true;
    private bool _saving;
    private bool _hoverAvatar;


    private bool IsUserNameUnchanged
        => string.IsNullOrWhiteSpace(_userNameInput)
           || _userNameInput == _user?.UserName;

    protected override async Task OnInitializedAsync()
    {
        _loading = true;

        _user = await UserService.GetMeAsync();
        if (_user is not null)
        {
            _userNameInput = _user.UserName;
        }

        _loading = false;
    }

    private async Task SaveUserNameAsync()
    {
        if (IsUserNameUnchanged)
        {
            return;
        }

        _saving = true;

        var userName = _userNameInput;
        var success = await UserService.UpdateUserNameAsync(userName);

        if (!success)
        {
            Snackbar.Add("Failed to update username", Severity.Error);
            _saving = false;
            return;
        }

        _user = _user! with { UserName = userName };
        Snackbar.Add("Username updated", Severity.Success);
        _saving = false;
    }

    private async Task OnAvatarSelectedAsync(InputFileChangeEventArgs e)
    {
        var file = e.File;
        if (file is null)
        {
            return;
        }

        const long maxSize = 5 * 1024 * 1024;
        if (file.Size > maxSize)
        {
            Snackbar.Add("Image is too large (max 5 MB)", Severity.Warning);
            return;
        }

        await using var stream = file.OpenReadStream(maxSize);

        var newUrl = await UserService.UpdateAvatarAsync(stream, file.Name);

        if (newUrl is null)
        {
            Snackbar.Add("Failed to upload avatar", Severity.Error);
            return;
        }

        _user = _user! with { AvatarUrl = newUrl };
        Snackbar.Add("Avatar updated", Severity.Success);
    }

    private async Task DeleteAvatarAsync()
    {
        var success = await UserService.DeleteAvatarAsync();

        if (!success)
        {
            Snackbar.Add("Failed to remove avatar", Severity.Error);
            return;
        }

        _user = await UserService.GetMeAsync();
        Snackbar.Add("Avatar removed", Severity.Success);
    }
}