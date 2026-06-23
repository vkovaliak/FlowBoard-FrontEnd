using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.Shared;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.Comments;

public partial class CommentComposer
{
    [Inject] public IAttachmentService AttachmentService { get; set; } = default!;

    [Parameter] public Guid BoardId { get; set; }
    [Parameter] public Guid CardId { get; set; }
    [Parameter] public Func<string, Task<Guid?>> OnCreateComment { get; set; } = default!;

    private AttachmentUploader _uploader = default!;
    private string _message = string.Empty;
    private bool _useRichEditor;
    private bool _isSubmitting;

    private void Cancel()
    {
        _message = string.Empty;
        _uploader?.Clear();
    }

    private async Task SubmitAsync()
    {
        if (string.IsNullOrWhiteSpace(_message))
        {
            return;
        }

        _isSubmitting = true;

        var commentId = await OnCreateComment(_message);

        if (commentId is not null && _uploader.HasPendingFiles)
        {
            await _uploader.UploadAllAsync(
                file => UploadToCommentAsync(commentId.Value, file));
        }

        _isSubmitting = false;
        Cancel();
    }

    private async Task UploadToCommentAsync(Guid commentId, IBrowserFile file)
    {
        await using var stream = file.OpenReadStream(50 * 1024 * 1024);
        await AttachmentService.UploadCommentAttachmentAsync(
            BoardId, CardId, commentId, stream, file.Name, file.ContentType);
    }

    private void EnableRichEditor()
    {
        _useRichEditor = true;
    }

    private void DisableRichEditor()
    {
        _useRichEditor = false;
    }
}