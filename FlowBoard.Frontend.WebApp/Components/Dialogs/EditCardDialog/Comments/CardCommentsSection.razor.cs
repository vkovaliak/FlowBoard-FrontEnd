using Microsoft.AspNetCore.Components;
using FlowBoard.Frontend.Domain.DTOs.Comments;
using FlowBoard.Frontend.Services.Abstractions;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.Comments;

public partial class CardCommentsSection : ComponentBase, IAsyncDisposable
{
    [Inject] public ICommentService CommentService { get; set; } = default!;
    [Inject] public ICommentHubService CommentHub { get; set; } = default!;
    [Inject] public IAttachmentService AttachmentService { get; set; } = default!;

    [Parameter] public Guid CardId { get; set; }

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
        _comments = await CommentService.GetCommentsAsync(CardId);
        _isLoading = false;
    }

    private async void HandleHubUpdate(Guid _)
    {
        _comments = await CommentService.GetCommentsAsync(CardId);
        await InvokeAsync(StateHasChanged);
    }

    private async Task<Guid?> CreateCommentAsync(string message)
    {
        var createdCommentId = await CommentService.CreateAsync(
            CardId, new CreateCommentDto(message));
        return createdCommentId;
    }

    private async Task UpdateCommentAsync((Guid Id, string Message) args)
        => await CommentService.UpdateAsync(
            CardId, args.Id, new UpdateCommentDto(args.Message));

    private async Task DeleteCommentAsync(Guid commentId)
        => await CommentService.DeleteAsync(CardId, commentId);

    public async ValueTask DisposeAsync()
    {
        CommentHub.OnCommentUpdated -= HandleHubUpdate;
        await CommentHub.LeaveCardCommentsAsync(CardId);
    }

    private async Task DeleteCommentAttachmentAsync(Guid attachmentId)
    {
        var success = await AttachmentService.DeleteCommentAttachmentAsync(
            attachmentId);

        if (success)
        {
            _comments = await CommentService.GetCommentsAsync(CardId);
            StateHasChanged();
        }
    }
}