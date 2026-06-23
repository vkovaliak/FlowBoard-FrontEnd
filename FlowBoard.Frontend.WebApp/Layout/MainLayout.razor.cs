using FlowBoard.Frontend.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using FlowBoard.Frontend.Domain.DTOs.Users;

namespace FlowBoard.Frontend.WebApp.Layout;

public partial class MainLayout
{
    [Inject] public IAuthService AuthService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public IUserService UserService { get; set; } = default!;

    private UserDto? _currentUser;
    private bool _isUserMenuOpen;


    protected override async Task OnInitializedAsync()
    {
        _currentUser = await UserService.GetMeAsync();
    }

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

    private void HandleProfileAsync()
    {
        NavigationManager.NavigateTo("/account");
    }

    private void ToggleUserMenu()
    {
        _isUserMenuOpen = !_isUserMenuOpen;
    }
}