using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using MudBlazor;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Services.Providers;
using FlowBoard.Frontend.Services.State;
using FlowBoard.Frontend.Domain.Authorization;
using FlowBoard.Frontend.Domain.Enums;
using Microsoft.Extensions.Options;
using FlowBoard.Frontend.Services.Configurations;
using FlowBoard.Frontend.Domain.Models.Cards;
using FlowBoard.Frontend.WebApp.Components.Dialogs.BoardFilterDialog;
using FlowBoard.Frontend.Domain.DTOs.Cards;
using FlowBoard.Frontend.Services.Helpers;

namespace FlowBoard.Frontend.WebApp.Pages.BoardDetails;

[Authorize]
public partial class BoardDetails : IAsyncDisposable
{
    [Parameter]
    public Guid Id { get; set; }

    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public CustomAuthStateProvider AuthStateProvider { get; set; } = default!;
    [Inject] public IOptions<MeetingOptions> MeetingOptions { get; set; } = default!;
    [Inject] public PresenceState PresenceState { get; set; } = default!;
    [Inject] public IBoardService BoardService { get; set; } = default!;
    [Inject] public ICardService CardService { get; set; } = default!;
    [Inject] public IListService ListService { get; set; } = default!;
    [Inject] public ISnackbar Snackbar { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public IJSRuntime JsRuntime { get; set; } = default!;
    [Inject] public IBoardHubService BoardHub { get; set; } = default!;
    [Inject] public FavoritesState FavoritesState { get; set; } = default!;

    private CardFilterModel _filter = new();

    private BoardDetailsDto? _board;
    private bool _isNotFound = false;
    private Guid _currentUserId;
    private Guid _loadedBoardId;
    private BoardViewTab _activeTab = BoardViewTab.Board;


    private DotNetObjectReference<BoardDetails>? _objRef;

    protected override async Task OnParametersSetAsync()
    {
        if (Id == _loadedBoardId)
        {
            return;
        }

        if (_loadedBoardId != Guid.Empty)
        {
            BoardHub.OnBoardUpdated -= HandleBoardHubUpdate;
            BoardHub.OnUserOnline -= HandleUserOnline;
            BoardHub.OnUserOffline -= HandleUserOffline;
            BoardHub.OnOnlineUsers -= HandleOnlineUsers;
            PresenceState.OnChanged -= StateChanged;
            PresenceState.Clear();

            await BoardHub.LeaveBoardAsync(_loadedBoardId);
            _objRef?.Dispose();
            _objRef = null;
        }

        _loadedBoardId = Id;
        _isNotFound = false;
        _board = null;

        _currentUserId = await AuthStateProvider
            .GetCurrentUserIdAsync();
        await RefreshBoardAsync();

        if (_board is null)
        {
            _isNotFound = true;
            Snackbar.Add("Board not found or access denied.", Severity.Error);
        }
        else
        {
            BoardHub.OnBoardUpdated += HandleBoardHubUpdate;
            BoardHub.OnUserOnline += HandleUserOnline;
            BoardHub.OnUserOffline += HandleUserOffline;
            BoardHub.OnOnlineUsers += HandleOnlineUsers;
            PresenceState.OnChanged += StateChanged;

            await BoardHub.ConnectAsync();
            await BoardHub.JoinBoardAsync(Id);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_board != null 
            && _activeTab == BoardViewTab.Board
            && BoardPermissions.CanModifyContent(_board.UserRole))
        {
            _objRef ??= DotNetObjectReference.Create(this);
            await JsRuntime.InvokeVoidAsync("initKanbanSortable", _objRef);
        }
    }

    private async void HandleBoardHubUpdate(Guid updatedBoardId)
    {
        if (updatedBoardId == Id)
        {
            await RefreshBoardAsync();
            await InvokeAsync(StateHasChanged);
        }
    }

    private void HandleUserOnline(Guid id) => PresenceState.Add(id);

    private void HandleUserOffline(Guid id) => PresenceState.Remove(id);

    private void HandleOnlineUsers(IReadOnlyCollection<Guid> users)
        => PresenceState.SetOnline(users);
        
    private void StateChanged() => InvokeAsync(StateHasChanged);

    private async Task RefreshBoardAsync()
    {
        _board = await BoardService.GetDetailsAsync(Id);
    }

    private void ShowResult(bool success, string successMsg, string errorMsg)
    {
        Snackbar.Add(
            success ? successMsg : errorMsg,
            success ? Severity.Success : Severity.Error);
    }

    private async Task<bool> ConfirmDeleteAsync(string title, string message)
    {
        return await DialogService.ShowMessageBoxAsync(
            title, message,
            yesText: "Delete",
            cancelText: "Cancel") == true;
    }

    private async Task HandleMembersChangedAsync()
    {
        await RefreshBoardAsync();

        var stillMember = _board?.Members.Any(
            m => m.UserId == _currentUserId) ?? false;
        if (!stillMember)
        {
            NavigationManager.NavigateTo("/");
            return;
        }

        StateHasChanged();
    }

    public async ValueTask DisposeAsync()
    {
        _objRef?.Dispose();
        BoardHub.OnBoardUpdated -= HandleBoardHubUpdate;
        BoardHub.OnUserOnline -= HandleUserOnline;
        BoardHub.OnUserOffline -= HandleUserOffline;
        BoardHub.OnOnlineUsers -= HandleOnlineUsers;
        PresenceState.OnChanged -= StateChanged;
        PresenceState.Clear();
        await BoardHub.LeaveBoardAsync(Id);
    }

    private async Task ToggleFavoriteAsync()
    {
        if (_board is null) return;

        var result = await BoardService.ToggleFavoriteAsync(_board.Id);

        if (result.Success)
        {
            await RefreshBoardAsync();
            FavoritesState.NotifyChanged();
        }
        else
        {
            Snackbar.Add(result.Error ?? "Failed", Severity.Error);
        }
    }

    private string PaperBackgroundStyle
    {
        get
        {
            if (string.IsNullOrEmpty(_board?.Background))
            {
                return string.Empty;
            }

            return $"background-image: url('{_board.Background}');" + 
                   "background-size: 100% 100%; background-position: center;";
        }
    }

    private void HandleTabChanged(BoardViewTab tab)
    {
        _activeTab = tab;
        StateHasChanged();
    }

    private void OpenMeeting()
    {
        if (_board == null)
        {
            return;
        }

        var me = _board.Members
            .FirstOrDefault(m => m.UserId == _currentUserId);

        var username = me?.UserName ?? "Guest";

        var roomId = _board.Id.ToString();

        var baseUrl = MeetingOptions.Value.Url.TrimEnd('/');
        var encodedName = Uri.EscapeDataString(username);
        var url = $"{baseUrl}/call/{roomId}?name={encodedName}";

        JsRuntime.InvokeVoidAsync("openInNewTab", url);
    }

    private async Task ArchiveBoardAsync()
    {
        if (_board is null)
        {
            return;
        }

        bool? confirm = await DialogService.ShowMessageBoxAsync(
            "Archive Board",
            $"Are you sure you want to archive board '{_board.Name}'?",
            yesText: "Archive",
            cancelText: "Cancel");

        if (confirm != true)
        {
            return;
        }

        var result = await BoardService.ArchiveBoardAsync(_board.Id);

        if (result.Success)
        {
            Snackbar.Add($"Board '{_board.Name}' archived", Severity.Success);
            NavigationManager.NavigateTo("/");
        }
        else
        {
            Snackbar.Add(result.Error ?? "Failed", Severity.Error);
        }
    }

    private async Task OpenFilterAsync()
    {
        var labels = _board!.Lists
            .SelectMany(l => l.Cards ?? [])
            .SelectMany(c => c.Labels)
            .DistinctBy(l => l.Id)
            .OrderBy(l => l.Name)
            .ToList();
            
        var parameters = new DialogParameters<BoardFilterDialog>
        {
            { x => x.Filter, _filter },
            { x => x.Labels, labels }
        };

        var options = new DialogOptions
        {
            Position = DialogPosition.CenterRight,
            FullWidth = false,
            CloseButton = false,
            NoHeader = true
        };

        var dialog = await DialogService.ShowAsync<BoardFilterDialog>(
            null, parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled && result.Data is CardFilterModel applied)
        {
            _filter = applied;
            StateHasChanged();
        }
    }

    private bool CardMatchesFilter(CardDto card)
        => CardFilterHelper.Matches(card, _filter, _currentUserId);
}