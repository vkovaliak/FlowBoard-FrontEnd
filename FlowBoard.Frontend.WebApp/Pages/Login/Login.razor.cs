using FlowBoard.Frontend.Domain.DTOs.Auth;
using FlowBoard.Frontend.Domain.Models.Auth;
using FlowBoard.Frontend.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Pages.Login;

public partial class Login
{
    [Inject] public IAuthService AuthService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public ISnackbar Snackbar { get; set; } = default!;

    private LoginModel Model { get; set; } = new();
    private bool _showPassword;
    private bool _rememberMe;
    private bool _isLoading;

    private void TogglePasswordVisibility() 
        => _showPassword = !_showPassword;

    private async Task HandleLogin()
    {
        var loginDto = new UserLoginDto(Model.Email, Model.Password);
        var result = await AuthService.LoginAsync(loginDto);

        if (result.Success)
        {
            Snackbar.Add("Login success!", Severity.Success);
            NavigationManager.NavigateTo("/");
        }
        else
        {
            Snackbar.Add($"Login failed: {result.Error}", Severity.Error);
        }
    }

    private async Task SignInWithMicrosoft()
    {
        _isLoading = true;
        StateHasChanged();

        try
        {
            var result = await AuthService.LoginWithMicrosoftAsync();
            
            if (result.Success)
            {
                Snackbar.Add("Microsoft login success!", Severity.Success);
                NavigationManager.NavigateTo("/");
            }
            else
            {
                Snackbar.Add($"Microsoft login failed: {result.Error}", Severity.Error);
            }
        }
        finally
        {
            _isLoading = false;
            StateHasChanged();
        }
    }
}