using MudBlazor;
using FlowBoard.Frontend.Domain.DTOs.Auth;
using Microsoft.AspNetCore.Components;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Domain.Models.Auth;

namespace FlowBoard.Frontend.WebApp.Pages;

public partial class Login
{
    [Inject] public IAuthService AuthService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public ISnackbar Snackbar { get; set; } = default!;
    
    private LoginModel Model { get; set; } = new();

    private async Task HandleLogin()
    {        
        var loginDto = new UserLoginDto(
            Email: Model.Email,
            Password: Model.Password
        );
        
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