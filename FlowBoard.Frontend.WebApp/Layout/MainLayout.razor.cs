using FlowBoard.Frontend.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using FlowBoard.Frontend.Domain.DTOs.Users;
using FlowBoard.Frontend.Services.State;

namespace FlowBoard.Frontend.WebApp.Layout;

public partial class MainLayout : IDisposable
{
    [Inject] public IAuthService AuthService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public IUserService UserService { get; set; } = default!;
    [Inject] public UserState UserState { get; set; } = default!;

    private UserDto? _currentUser;
    private bool _isUserMenuOpen;


    protected override async Task OnInitializedAsync()
    {
        UserState.OnChanged += HandleUserChanged;
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

    private async void HandleUserChanged()
    {
        _currentUser = await UserService.GetMeAsync();
        await InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        UserState.OnChanged -= HandleUserChanged;
    }
}