using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using FlowBoard.Frontend.Services.Abstractions;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.Comments;

public partial class CommentComposer
{
    [Inject] public IAttachmentService AttachmentService { get; set; } = default!;

    [Parameter] public Guid BoardId { get; set; }
    [Parameter] public Guid CardId { get; set; }
    [Parameter] public Func<string, Task<Guid?>> OnCreateComment { get; set; } = default!;

    private readonly List<IBrowserFile> _pendingFiles = [];
    private string? _uploadingFile;
    private readonly string _inputId = $"comment-file-{Guid.NewGuid():N}";

    private string _message = string.Empty;
    private bool _useRichEditor;
    private bool _isSubmitting;

    private void OnFilesSelected(InputFileChangeEventArgs e)
    {
        _pendingFiles.AddRange(e.GetMultipleFiles());
    }

    private void RemoveFile(IBrowserFile file)
    {
        _pendingFiles.Remove(file);
    }

    private void ClearAll()
    {
        _message = string.Empty;
        _pendingFiles.Clear();
        _uploadingFile = null;
    }

    private void ToggleRichEditor()
    {
        _useRichEditor = !_useRichEditor;
    }

    private async Task SubmitAsync()
    {
        if (string.IsNullOrWhiteSpace(_message))
        {
            return;
        }

        _isSubmitting = true;
        StateHasChanged();

        try
        {
            var commentId = await OnCreateComment(_message);

            if (commentId is null)
            {
                return;
            }

            foreach (var file in _pendingFiles.ToList())
            {
                _uploadingFile = file.Name;
                StateHasChanged();

                await UploadToCommentAsync(commentId.Value, file);
            }

            ClearAll();
        }
        finally
        {
            _isSubmitting = false;
            StateHasChanged();
        }
    }

    private async Task UploadToCommentAsync(
        Guid commentId, IBrowserFile file)
    {
        await using var stream = file.OpenReadStream(
            50 * 1024 * 1024);

        await AttachmentService.UploadCommentAttachmentAsync(
            BoardId, CardId, commentId, stream, 
            file.Name, file.ContentType);
    }

    private static string GetFileIcon(string contentType) =>
        contentType.StartsWith("image/")
            ? Icons.Material.Filled.Image
            : Icons.Material.Filled.InsertDriveFile;

    private static string FormatSize(long bytes) => bytes switch
    {
        < 1024 => $"{bytes} B",
        < 1024 * 1024 => $"{bytes / 1024.0:F1} KB",
        _ => $"{bytes / (1024.0 * 1024):F1} MB"
    };
}