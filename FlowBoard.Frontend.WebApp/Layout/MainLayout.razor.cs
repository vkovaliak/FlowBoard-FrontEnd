using FlowBoard.Frontend.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using FlowBoard.Frontend.Domain.DTOs.Users;
using FlowBoard.Frontend.Services.State;
using MudBlazor;
using FlowBoard.Frontend.WebApp.Components.Dialogs.CreateBoardDialog;
using FlowBoard.Frontend.Domain.Models.Boards;
using FlowBoard.Frontend.Domain.DTOs.Boards;

namespace FlowBoard.Frontend.WebApp.Layout;

public partial class MainLayout : IDisposable
{
    [Inject] public IAuthService AuthService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public IUserService UserService { get; set; } = default!;
    [Inject] public UserState UserState { get; set; } = default!;

    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public IBoardService BoardService { get; set; } = default!;
    [Inject] public ISnackbar Snackbar { get; set; } = default!;

    private UserDto? _currentUser;
    private bool _isUserMenuOpen;

    private bool _isChatOpen;

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

    private async Task OpenCreateBoardDialogAsync()
    {
        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        var dialog = await DialogService.ShowAsync<CreateBoardDialog>(
            "Create New Board", options);

        var result = await dialog.Result;

        if (result is null || result.Canceled)
        {
            return;
        }

        if (result.Data is not CreateBoardModel model)
        {
            return;
        }

        var createModel = new CreateBoardDto(model.Name, model.IsPublic);

        var createResult = await BoardService.CreateAsync(createModel);

        if (!createResult.Success)
        {
            Snackbar.Add(createResult.Error ?? "Failed to create board", 
                Severity.Error);
            return;
        }

        Snackbar.Add("Board created", Severity.Success);

        NavigationManager.NavigateTo($"/boards/{createResult.Value}");
    }

    public void Dispose()
    {
        UserState.OnChanged -= HandleUserChanged;
    }

    private void ToggleChat()
    {
        _isChatOpen = !_isChatOpen;
    }
}