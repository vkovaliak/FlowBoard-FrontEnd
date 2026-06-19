using Microsoft.AspNetCore.Components;
using FlowBoard.Frontend.Domain.DTOs.Comments;
using Microsoft.AspNetCore.Components.Authorization;
using FlowBoard.Frontend.Services.Providers;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.Comments;

public partial class CommentItem
{
    [Inject] AuthenticationStateProvider AuthStateProvider { get; set; } = default!;
    [Parameter] public CommentDto Comment { get; set; } = default!;
    [Parameter] public EventCallback<(Guid Id, string Message)> OnUpdate { get; set; }
    [Parameter] public EventCallback<Guid> OnDelete { get; set; }

    [Parameter] public EventCallback<Guid> OnDeleteAttachment { get; set; }

    private bool CanManage 
        => _currentUserId == Comment.CreatedBy;
        
    private bool _isEditing;
    private string _editMessage = string.Empty;
    private Guid _currentUserId;

    protected override async Task OnInitializedAsync()
    {
        if (AuthStateProvider is CustomAuthStateProvider customProvider)
        {
            _currentUserId = await customProvider.GetCurrentUserIdAsync();
        }
    }

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