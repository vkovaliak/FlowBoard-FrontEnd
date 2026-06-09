using FlowBoard.Frontend.Services.Abstractions;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Layout;

public partial class MainLayout
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
}