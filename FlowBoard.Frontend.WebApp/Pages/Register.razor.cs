using MudBlazor;
using FlowBoard.Frontend.Domain.DTOs.Auth;
using Microsoft.AspNetCore.Components;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Domain.Models.Auth;

namespace FlowBoard.Frontend.WebApp.Pages;

public partial class Register
{
    [Inject] public IAuthService AuthService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public ISnackbar Snackbar { get; set; } = default!;
    
    private RegisterModel Model { get; set; } = new();

    private async Task HandleRegister()
    {        
        var registerDto = new UserRegisterDto(
            Email: Model.Email,
            Password: Model.Password
        );
        
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