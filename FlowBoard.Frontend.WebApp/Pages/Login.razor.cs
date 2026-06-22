using FlowBoard.Frontend.Domain.DTOs.Auth;
using FlowBoard.Frontend.Domain.Models.Auth;
using FlowBoard.Frontend.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Pages;

public partial class Login
{
    [Inject] public IAuthService AuthService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public ISnackbar Snackbar { get; set; } = default!;

    private LoginModel Model { get; set; } = new();
    private bool _showPassword;
    private bool _rememberMe;

    private void TogglePasswordVisibility() 
        => _showPassword = !_showPassword;

    private async Task HandleLogin()
    {
        var loginDto = new UserLoginDto(Model.Email, Model.Password);
        var result = await AuthService.LoginAsync(loginDto);

        if (result)
        {
            Snackbar.Add("Login success!", Severity.Success);
            NavigationManager.NavigateTo("/");
        }
        else
        {
            Snackbar.Add("Login failed", Severity.Error);
        }
    }
}