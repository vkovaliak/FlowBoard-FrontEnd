using FlowBoard.Frontend.Domain.DTOs.Auth;
using FlowBoard.Frontend.Domain.Models.Auth;
using FlowBoard.Frontend.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Pages;

public partial class Register
{
    [Inject] public IAuthService AuthService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public ISnackbar Snackbar { get; set; } = default!;

    private RegisterModel Model { get; set; } = new();
    private bool _showPassword;

    private bool HasMinLength 
        => Model.Password.Length >= 6;

    private void TogglePasswordVisibility() 
        => _showPassword = !_showPassword;

    private async Task HandleRegister()
    {
        if (!HasMinLength)
        {
            Snackbar.Add(
                "Password does not meet requirements", Severity.Warning);
            return;
        }

        var registerDto = new UserRegisterDto(
            Email: Model.Email,
            Password: Model.Password,
            UserName: Model.UserName);

        var result = await AuthService.RegisterAsync(registerDto);

        if (result)
        {
            Snackbar.Add("Register success!", Severity.Success);
            NavigationManager.NavigateTo("/");
        }
        else
        {
            Snackbar.Add("Register failed", Severity.Error);
        }
    }
}