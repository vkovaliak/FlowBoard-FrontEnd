using FlowBoard.Frontend.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using FlowBoard.Frontend.WebApp.Theme;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Layout;

public partial class MainLayout
{
    [Inject] public IAuthService AuthService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    private bool _drawerOpen = true;

    private void ToggleDrawer()
    {
        _drawerOpen = !_drawerOpen;
    }

    private async Task HandleLogoutAsync()
    {
        await AuthService.LogoutAsync();
        NavigationManager.NavigateTo("/login");
    }
}