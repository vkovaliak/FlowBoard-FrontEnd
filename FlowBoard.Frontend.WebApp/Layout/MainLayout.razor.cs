using FlowBoard.Frontend.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using FlowBoard.Frontend.Domain.DTOs.Users;
using FlowBoard.Frontend.Services.State;
using MudBlazor;
using FlowBoard.Frontend.WebApp.Components.Dialogs.CreateBoardDialog;
using FlowBoard.Frontend.Domain.Models.Boards;
using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Services.Handlers;
using Microsoft.JSInterop;
using FlowBoard.Frontend.WebApp.Components.Dialogs.HotkeysHelpDialog;

namespace FlowBoard.Frontend.WebApp.Layout;

public partial class MainLayout : IAsyncDisposable
{
    [Inject] public IAuthService AuthService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public IUserService UserService { get; set; } = default!;
    [Inject] public UserState UserState { get; set; } = default!;
    [Inject] private UpgradeHandler UpgradeHandler { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public IBoardService BoardService { get; set; } = default!;
    [Inject] public ISnackbar Snackbar { get; set; } = default!;
    [Inject] public NotificationState NotificationState { get; set; } = default!;

    private UserDto? _currentUser;
    private bool _isUserMenuOpen;
    private bool _isChatOpen;
    private bool _drawerOpen = true;
    private DotNetObjectReference<MainLayout>? _hotkeyRef;
    private bool _isHelpOpen;

    protected override async Task OnInitializedAsync()
    {
        UserState.OnChanged += HandleUserChanged;
        _currentUser = await UserService.GetMeAsync();
        await UserState.EnsureLoadedAsync();
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _hotkeyRef = DotNetObjectReference.Create(this);
            await JSRuntime.InvokeVoidAsync(
                "hotkeys.register", _hotkeyRef);
        }
    }

    private void ToggleDrawer()
    {
        _drawerOpen = !_drawerOpen;
    }

    private async Task HandleLogoutAsync()
    {
        UserState.Clear();
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

        var createModel = new CreateBoardDto(
            model.Name, 
            model.IsPublic, 
            model.Background, 
            model.BoardTemplate);

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

    public async ValueTask DisposeAsync()
    {
        UserState.OnChanged -= HandleUserChanged;

        if (_hotkeyRef is not null)
        {
            await JSRuntime.InvokeVoidAsync("hotkeys.unregister");
            _hotkeyRef.Dispose();
        }
    }

    private void ToggleChat()
    {
        if (!UserState.IsPro)
        {
            return;
        }
        _isChatOpen = !_isChatOpen;
    }

    private async Task Upgrade() 
        => await UpgradeHandler.StartUpgradeAsync();
    
    private async Task FocusSearchAsync()
    {
        await JSRuntime.InvokeVoidAsync("hotkeys.focusSearch");
    }

    private void ToggleNotifications() 
        => NotificationState.Toggle();
    
    private async Task HandleEscape()
    {
        var searchFocused = await JSRuntime.InvokeAsync<bool>(
            "hotkeys.isSearchFocused");
        
        if(searchFocused)
        {
            await JSRuntime.InvokeVoidAsync("hotkeys.unFocusSearch");
        }

        if (_isHelpOpen)
        {
            _isHelpOpen = false;
            return;
        }

        if (_isUserMenuOpen)
        {
            _isUserMenuOpen = false;
            return;
        }

        if (NotificationState.IsOpen)
        {
            NotificationState.Close();
            return;
        }

        if (_isChatOpen)
        {
            _isChatOpen = false;
            return;
        }
    }

    private async Task ShowHelpAsync()
    {
        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.ExtraSmall,
            FullWidth = true,
            CloseButton = true
        };

        await DialogService.ShowAsync<HotkeysHelpDialog>(
            null, options);
    }
    
    [JSInvokable]
    public async Task OnHotkey(string action)
    {
        switch (action)
        {
            case "Search":
                await FocusSearchAsync();
                break;

            case "Notifications":
                ToggleNotifications();
                break;

            case "Escape":
                await HandleEscape();
                break;
            
            case "Help":
                await ShowHelpAsync();
                break;
            
            case "Chat":
                ToggleChat();
                break;
        }

        StateHasChanged();
    }
}