using FlowBoard.Frontend.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using FlowBoard.Frontend.WebApp.Theme;
using MudBlazor;


namespace FlowBoard.Frontend.WebApp.Layout;

public partial class AuthLayout
{
    [Inject] 
    public IAuthService AuthService { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;
    
    private async Task HandleLogoutAsync()
    {
        await AuthService.LogoutAsync();

        NavigationManager.NavigateTo("/login");
    }

    private MudTheme _theme = AppTheme.Build();
    private bool _isDarkMode = false;
    
}