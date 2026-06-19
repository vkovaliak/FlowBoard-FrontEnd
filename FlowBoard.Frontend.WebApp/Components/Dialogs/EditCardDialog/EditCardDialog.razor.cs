using Microsoft.AspNetCore.Components;
using MudBlazor;
using FlowBoard.Frontend.Domain.DTOs.Cards;
using FlowBoard.Frontend.Domain.Models.Cards;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Domain.DTOs.Attachments;
using FlowBoard.Frontend.Domain.DTOs.Boards;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog;

public partial class EditCardDialog : ComponentBase, IAsyncDisposable
{
    [Inject] public IAttachmentService AttachmentService { get; set; } = default!;
    [Inject] public IBoardService BoardService { get; set; } = default!;
    [Inject] public IBoardHubService BoardHub { get; set; } = default!;

    [CascadingParameter] public IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public Guid BoardId { get; set; }
    [Parameter] public Guid CardId { get; set; }

    private BoardDetailsDto? _board;
    private CardDto? _card;
    private CreateCardModel _model = new();
    private bool _isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        await LoadCardAsync(isInitial: true);

        BoardHub.OnBoardUpdated += HandleBoardUpdated;
        await BoardHub.ConnectAsync();
        await BoardHub.JoinBoardAsync(BoardId);
    }

    private async Task LoadCardAsync(bool isInitial = false)
    {
        _board = await BoardService.GetDetailsAsync(BoardId);

        _card = _board?.Lists
            .SelectMany(l => l.Cards ?? [])
            .FirstOrDefault(c => c.Id == CardId);

        if (isInitial && _card is not null)
        {
            _model.Name = _card.Name;
            _model.Description = _card.Description;
            _model.DueDate = _card.DueDate;
        }

        _isLoading = false;
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

    private void Cancel() => MudDialog.Cancel();

    private void Save()
    {
        if (string.IsNullOrWhiteSpace(_model.Name))
        {
            return;
        }

        MudDialog.Close(DialogResult.Ok(
            new UpdateCardDto(
                _model.Name,
                _model.Description ?? string.Empty,
                _model.DueDate)));
    }

    private async Task OnChanged() => await Task.CompletedTask;

    public async ValueTask DisposeAsync()
    {
        BoardHub.OnBoardUpdated -= HandleBoardUpdated;
        await BoardHub.LeaveBoardAsync(BoardId);
    }
}