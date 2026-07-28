using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Domain.DTOs.Boards;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.Comments;

public partial class CommentComposer
{
    [Inject] public IAttachmentService AttachmentService { get; set; } = default!;

    [Parameter] public Guid BoardId { get; set; }
    [Parameter] public Guid CardId { get; set; }
    [Parameter] public Func<string, Guid?, Task<Guid?>> OnCreateComment { get; set; } = default!;
    [Parameter] public List<BoardMemberAvatarDto> Members { get; set; } = [];

    private readonly List<IBrowserFile> _pendingFiles = [];
    private string? _uploadingFile;
    private readonly string _inputId = $"comment-file-{Guid.NewGuid():N}";

    private string _message = string.Empty;
    private bool _useRichEditor;
    private bool _isSubmitting;
    private bool _showMentions;
    private string _mentionQuery = string.Empty;
    private int _mentionStartIndex = -1;
    private List<BoardMemberAvatarDto> _filteredMembers = [];

    private Guid? _mentionedUserId;

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
        _mentionedUserId = null;
        _showMentions = false;
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
            var (html, userId) = BuildMessage();

            var commentId = await OnCreateComment(html, userId);

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

    private void OnMessageChanged(string value)
    {
        _message = value;
        DetectMention();
    }

    private void DetectMention()
    {
        var lastAt = _message.LastIndexOf('@');

        if (lastAt < 0)
        {
            _showMentions = false;
            return;
        }

        var afterAt = _message.Substring(lastAt + 1);

        if (afterAt.Contains(' ') || afterAt.Contains('\n'))
        {
            _showMentions = false;
            return;
        }

        _mentionStartIndex = lastAt;
        _mentionQuery = afterAt;

        _filteredMembers = Members
            .Where(m => m.UserName.Contains(
                _mentionQuery, StringComparison.OrdinalIgnoreCase))
            .Take(6)
            .ToList();

        _showMentions = _filteredMembers.Count > 0;
    }

    private void SelectMention(BoardMemberAvatarDto member)
    {
        var before = _message.Substring(0, _mentionStartIndex);
        var mentionText = $"@{member.UserName}";

        _message = $"{before}{mentionText} ";

        _mentionedUserId = member.UserId;

        _showMentions = false;
    }

    private (string Html, Guid? UserId) BuildMessage()
    {
        if (_mentionedUserId is null)
        {
            return (_message, null);
        }

        var member = Members.FirstOrDefault(
            m => m.UserId == _mentionedUserId);

        if (member is null)
        {
            return (_message, null);
        }

        var mentionText = $"@{member.UserName}";

        if (!_message.Contains(mentionText))
        {
            return (_message, null);
        }

        var html = _message.Replace(
            mentionText, $"<b>{mentionText}</b>");

        return (html, _mentionedUserId);
    }
}