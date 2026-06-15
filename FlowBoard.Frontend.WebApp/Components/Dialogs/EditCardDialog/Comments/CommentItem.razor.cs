using Microsoft.AspNetCore.Components;
using FlowBoard.Frontend.Domain.DTOs.Comments;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.Comments;

public partial class CommentItem
{
    [Parameter] public CommentDto Comment { get; set; } = default!;
    [Parameter] public EventCallback<(Guid Id, string Message)> OnUpdate { get; set; }
    [Parameter] public EventCallback<Guid> OnDelete { get; set; }

    [Parameter] public EventCallback<Guid> OnDeleteAttachment { get; set; }

    private bool _isEditing;
    private string _editMessage = string.Empty;

    private void StartEdit()
    {
        _editMessage = Comment.Message;
        _isEditing = true;
    }

    private void CancelEdit()
    {
        _isEditing = false;
        _editMessage = string.Empty;
    }

    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(_editMessage))
        {
            return;
        }  

        await OnUpdate.InvokeAsync((Comment.Id, _editMessage));
        CancelEdit();
    }
}