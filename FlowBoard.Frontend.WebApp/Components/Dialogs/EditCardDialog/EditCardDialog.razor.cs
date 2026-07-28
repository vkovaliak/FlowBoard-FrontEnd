using Microsoft.AspNetCore.Components;
using MudBlazor;
using FlowBoard.Frontend.Domain.Authorization;
using FlowBoard.Frontend.Domain.DTOs.Cards;
using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Providers;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog;

public partial class EditCardDialog : ComponentBase, IAsyncDisposable
{
    [Inject] public IAttachmentService AttachmentService { get; set; } = default!;
    [Inject] public IBoardService BoardService { get; set; } = default!;
    [Inject] public IBoardHubService BoardHub { get; set; } = default!;
    [Inject] CustomAuthStateProvider AuthStateProvider { get; set; } = default!;

    [CascadingParameter] public IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public Guid BoardId { get; set; }
    [Parameter] public Guid CardId { get; set; }

    private BoardDetailsDto? _board;
    private CardDto? _card;
    private int _activeTab = 0;
    private List<BoardMemberAvatarDto> _mentionMembers = [];
    private Guid _currentUserId;

    private bool CanEdit => _board is not null
        && BoardPermissions.CanModifyContent(_board.UserRole);

    protected override async Task OnInitializedAsync()
    {
        _currentUserId = await AuthStateProvider.GetCurrentUserIdAsync();
        await LoadCardAsync();
        BoardHub.OnBoardUpdated += HandleBoardUpdated;
    }

    private async Task LoadCardAsync()
    {
        _board = await BoardService.GetDetailsAsync(BoardId);

        _card = _board?.Lists
            .SelectMany(l => l.Cards ?? [])
            .FirstOrDefault(c => c.Id == CardId);
        
        _mentionMembers = _board?.Members
            .Where(m => m.UserId != _currentUserId)
            .Select(m => new BoardMemberAvatarDto(
                m.UserId, m.UserName, m.AvatarUrl))
            .ToList() ?? [];

    }

    private async void HandleBoardUpdated(Guid updatedBoardId)
    {
        if (updatedBoardId != BoardId)
        {
            return;
        }

        await LoadCardAsync();

        if (_card is null)
        {
            await InvokeAsync(MudDialog.Cancel);
            return;
        }

        await InvokeAsync(StateHasChanged);
    }

    private async Task DeleteCardAttachmentAsync(Guid attachmentId)
    {
        await AttachmentService.DeleteCardAttachmentAsync(
            BoardId, CardId, attachmentId);
    }

    private void Close() => MudDialog.Cancel();

    public ValueTask DisposeAsync()
    {
        BoardHub.OnBoardUpdated -= HandleBoardUpdated;
        return ValueTask.CompletedTask;
    }
}