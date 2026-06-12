using FlowBoard.Frontend.Domain.DTOs.Cards;
using FlowBoard.Frontend.Domain.DTOs.Comments;
using FlowBoard.Frontend.Domain.Models.Cards;
using FlowBoard.Frontend.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs;

public partial class EditCardDialog : ComponentBase, IAsyncDisposable
{
    [CascadingParameter] 
    public IMudDialogInstance MudDialog { get; set; } = default!;

    [Inject] 
    public ICommentService CommentService { get; set; } = default!;

    [Inject]
    public ICommentHubService CommentHub { get; set; } = default!;
    [Inject] 
    public IAttachmentService AttachmentService { get; set; } = default!;

    [Parameter] 
    public Guid CardId { get; set; }

    [Parameter] 
    public string CurrentName { get; set; } = string.Empty;

    [Parameter] 
    public string? CurrentDescription { get; set; }

    private CreateCardModel _model = new();

    
    private bool _isEditingDescription = false;
    private string _originalDescription = string.Empty;

    private IEnumerable<CommentDto> _comments = [];

    private string _newCommentMessage = string.Empty;
    private bool _isLoadingComments = false;        
    private bool _isWritingComment = false;
    private Guid? _editingCommentId = null;
    private string _editingCommentMessage = string.Empty;

    private void EnableDescriptionEditing()
    {
        _originalDescription = _model.Description ?? string.Empty;
        _isEditingDescription = true;
    }

    private void CancelDescriptionEditing()
    {
        _isEditingDescription = false;
    }

    protected override async Task OnInitializedAsync()
    {
        _model.Name = CurrentName;
        _model.Description = CurrentDescription;

        await LoadCommentsAsync();

        CommentHub.OnCommentUpdated += HandleNewComment;

        await CommentHub.ConnectAsync();
        await CommentHub.JoinCardCommentsAsync(CardId);
    }

    private void Cancel() 
        => MudDialog.Cancel();

    private void SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(_model.Name))
        {
            return;
        }
        
        MudDialog.Close(DialogResult.Ok(
            new UpdateCardDto(_model.Name, _model.Description!)));
    }

    private async Task LoadCommentsAsync()
    {
        _isLoadingComments = true;
        _comments = await CommentService.GetCommentsAsync(CardId);
        _isLoadingComments = false;
    }

    private async Task SendCommentAsync()
    {
        if (string.IsNullOrWhiteSpace(_newCommentMessage))
        {
            return;
        }

        var dto = new CreateCommentDto(_newCommentMessage);
        var success = await CommentService.CreateAsync(CardId, dto);

        if (success)
        {
            _newCommentMessage = string.Empty;
        }
    }

    private readonly Dictionary<string, object> _editorConfig = new()
    {
        { "height", 100 },
        { "menubar", false },
        { "statusbar", false },
        { "plugins", "lists link autoresize image" },
        { "toolbar", 
            "undo redo | " +
            "blocks | " +
            "bold italic underline | " +
            "bullist numlist | " +
            "alignleft aligncenter alignright |" +
            "link" }
    };

    private async void HandleNewComment(Guid commentId)
    {
        var comments = await CommentService.GetCommentsAsync(CardId); 
        _comments = comments; 
        await InvokeAsync(StateHasChanged);
    }

    public async ValueTask DisposeAsync()
    {
        CommentHub.OnCommentUpdated -= HandleNewComment;

        await CommentHub.LeaveCardCommentsAsync(CardId);
    }

    private void CancelNewComment()
    {
        _newCommentMessage = string.Empty;
        _isWritingComment = false;
    }

    private void StartEditComment(CommentDto comment)
    {
        _editingCommentId = comment.Id;
        _editingCommentMessage = comment.Message;
    }

    private void CancelEditComment()
    {
        _editingCommentId = null;
        _editingCommentMessage = string.Empty;
    }

    private async Task SaveCommentUpdateAsync()
    {
        if (_editingCommentId == null || 
            string.IsNullOrWhiteSpace(_editingCommentMessage))
        {
            return;
        }

        var dto = new UpdateCommentDto(_editingCommentMessage);
        
        var success = await CommentService.UpdateAsync(
            CardId, _editingCommentId.Value, dto);

        if (success)
        {
            CancelEditComment();
            _isWritingComment = false;
        }
    }

    private async Task DeleteCommentAsync(Guid commentId)
    {
        var success = await CommentService.DeleteAsync(CardId, commentId);

        if (success)
        {
            if (_editingCommentId == commentId)
            {
                CancelEditComment();
            }
        }
    }

    private async Task UploadAttachmentAsync(IBrowserFile file)
    {

        await using var stream = file.OpenReadStream(50 * 1024 * 1024);

        var uploadedUrl = await AttachmentService.UploadAttachmentAsync(
            stream,
            file.Name,
            file.ContentType);

        if (file.ContentType.StartsWith("image/"))
        {
            _model.Description +=
                $"<p><img src=\"{uploadedUrl}\" style=\"max-width:100%; border-radius:8px;\" /></p>";
        }
        else
        {
            _model.Description +=
                $"<p><a href=\"{uploadedUrl}\" target=\"_blank\">{file.Name}</a></p>";
        }
    }
        

    private async Task HandleFileSelected(InputFileChangeEventArgs e)
    {
        foreach (var file in e.GetMultipleFiles())
        {
            await UploadAttachmentAsync(file);
        }

        StateHasChanged();
    }
}