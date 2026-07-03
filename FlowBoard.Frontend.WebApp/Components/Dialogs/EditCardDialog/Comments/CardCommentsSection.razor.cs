using Microsoft.AspNetCore.Components;
using FlowBoard.Frontend.Domain.DTOs.Comments;
using FlowBoard.Frontend.Services.Abstractions;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.Comments;

public partial class CardCommentsSection : ComponentBase, IAsyncDisposable
{
    [Inject] public ICommentService CommentService { get; set; } = default!;
    [Inject] public ICommentHubService CommentHub { get; set; } = default!;
    [Inject] public IAttachmentService AttachmentService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public ISnackbar Snackbar { get; set; } = default!;

    [Parameter] public Guid BoardId { get; set; }
    [Parameter] public Guid CardId { get; set; }
    [Parameter] public bool CanEdit { get; set; } = true;

    private IEnumerable<CommentDto> _comments = [];
    private bool _isLoading;

    protected override async Task OnInitializedAsync()
    {
        await LoadAsync();

        CommentHub.OnCommentUpdated += HandleHubUpdate;
        await CommentHub.ConnectAsync();
        await CommentHub.JoinCardCommentsAsync(CardId);
    }

    private async Task LoadAsync()
    {
        _isLoading = true;
        _comments = await CommentService.GetCommentsAsync(
            BoardId, CardId);
        _isLoading = false;
    }

    private async void HandleHubUpdate(Guid _)
    {
        _comments = await CommentService.GetCommentsAsync(
            BoardId, CardId);
        await InvokeAsync(StateHasChanged);
    }

    private async Task<Guid?> CreateCommentAsync(string message)
    {
        var createdCommentId = await CommentService.CreateAsync(
            BoardId, CardId, new CreateCommentDto(message));
        return createdCommentId.Value;
    }

    private async Task UpdateCommentAsync((Guid Id, string Message) args)
        => await CommentService.UpdateAsync(
            BoardId, CardId, args.Id, new UpdateCommentDto(args.Message));

    private async Task DeleteCommentAsync(Guid commentId)
    {
        bool? confirmed = await DialogService.ShowMessageBoxAsync(
            "Delete Comment",
            "Are you sure you want to delete this comment?",
            yesText: "Delete!", cancelText:"Cancle");

            if (confirmed != true)
            {
                return;
            }

            var result = await CommentService.DeleteAsync(BoardId, CardId, commentId);

            if (!result.Success)
            {
                Snackbar.Add($"Deleted failed: {result.Error}", Severity.Error);
            }

            Snackbar.Add("Comment is deleted", Severity.Success);

    }

    public async ValueTask DisposeAsync()
    {
        CommentHub.OnCommentUpdated -= HandleHubUpdate;
        await CommentHub.LeaveCardCommentsAsync(CardId);
    }

    private async Task DeleteCommentAttachmentAsync(Guid attachmentId)
    {
        await AttachmentService.DeleteCommentAttachmentAsync(
            BoardId, CardId, attachmentId);

    }
}