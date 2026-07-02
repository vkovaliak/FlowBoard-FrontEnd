using FlowBoard.Frontend.Domain.DTOs.Users;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.State;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Pages.Account;

public partial class Account
{
    [Inject] private IUserService UserService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [Inject] private UserState UserState { get; set; } = default!;

    private UserDto? _user;
    private string _userNameInput = string.Empty;
    private bool _loading = true;
    private bool _saving;
    private bool _hoverAvatar;
    private string _currentPassword = string.Empty;
    private string _newPassword = string.Empty;
    private string _confirmPassword = string.Empty;
    private bool _showCurrent;
    private bool _showNew;
    private bool _changingPassword;

    private bool IsUserNameUnchanged
        => string.IsNullOrWhiteSpace(_userNameInput)
           || _userNameInput == _user?.UserName;
    
    private bool CanChangePassword
        => !string.IsNullOrWhiteSpace(_currentPassword)
            && !string.IsNullOrWhiteSpace(_newPassword)
            && _newPassword.Length >= 6
            && _newPassword == _confirmPassword
            && _newPassword != _currentPassword;

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
        var result = await UserService.UpdateUserNameAsync(userName);

        if (!result.Success)
        {
            Snackbar.Add(result.Error ?? "Failed", Severity.Error);
            _saving = false;
            return;
        }

        _user = _user! with { UserName = userName };
        UserState.NotifyChanged();
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

        _user = _user! with { AvatarUrl = newUrl.Value };
        UserState.NotifyChanged();
        Snackbar.Add("Avatar updated", Severity.Success);
    }

    private async Task DeleteAvatarAsync()
    {
        var result = await UserService.DeleteAvatarAsync();

        if (!result.Success)
        {
            Snackbar.Add(result.Error ?? "Failed", Severity.Error);
            return;
        }

        _user = await UserService.GetMeAsync();
        UserState.NotifyChanged();
        Snackbar.Add("Avatar removed", Severity.Success);
    }

    private async Task ChangePasswordAsync()
    {
        if (!CanChangePassword)
        {
            return;
        }

        _changingPassword = true;

        var dto = new ChangePasswordDto(
            _currentPassword, _newPassword, _confirmPassword);

        var result = await UserService.ChangePasswordAsync(dto);

        _changingPassword = false;

        if (!result.Success)
        {
            Snackbar.Add(result.Error ?? 
                "Failed to change password", Severity.Error);
            return;
        }

        _currentPassword = string.Empty;
        _newPassword = string.Empty;
        _confirmPassword = string.Empty;

        Snackbar.Add("Password changed successfully", Severity.Success);
    }
}