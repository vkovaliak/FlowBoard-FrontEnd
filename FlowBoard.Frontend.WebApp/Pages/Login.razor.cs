using MudBlazor;
using FlowBoard.Frontend.Domain.DTOs.Auth;
using Microsoft.AspNetCore.Components;
using FlowBoard.Frontend.Services.Abstractions;

namespace FlowBoard.Frontend.WebApp.Pages;

public partial class Login
{
    [Inject] public IAuthService AuthService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public ISnackbar Snackbar { get; set; } = default!;
    
    private MudForm form = null!;
    private string? password;
    private string? email;

    private async Task HandleLogin()
    {        
        var loginDto = new UserLoginDto(
            Email: email ?? string.Empty,
            Password: password ?? string.Empty
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