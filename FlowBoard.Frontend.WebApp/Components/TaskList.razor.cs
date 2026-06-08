using FlowBoard.Frontend.Domain.DTOs.Lists;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components;

public partial class TaskList
{
    [Parameter]
    public ListDto List { get; set; } = default!;

    [Parameter] 
    public EventCallback<(Guid ListId, string NewName)> OnRenameList { get; set; }

    [Parameter] 
    public EventCallback<(Guid ListId, string ListName)> OnDeleteList { get; set; }

    private bool _isEditingName = false;
    private string _editedName = string.Empty;

    private void StartRename()
    {
        _editedName = List.Name;
        _isEditingName = true;
    }

    private void CancelRename()
    {
        _isEditingName = false;
    }

    private async Task SaveRenameAsync()
    {
        if (!string.IsNullOrWhiteSpace(_editedName) && _editedName != List.Name)
        {
            await OnRenameList.InvokeAsync((List.Id, _editedName));
        }
        _isEditingName = false;
    }

    private async Task DeleteListClickAsync()
    {
        await OnDeleteList.InvokeAsync((List.Id, List.Name));
    }
}