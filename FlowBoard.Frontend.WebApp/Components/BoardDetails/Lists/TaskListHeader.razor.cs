using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.BoardDetails.Lists;

public partial class TaskListHeader
{
    [Parameter] public string ListName { get; set; } = string.Empty;
    [Parameter] public int CardsCount { get; set; }

    [Parameter] public EventCallback OnAddCardClick { get; set; }
    [Parameter] public EventCallback OnDeleteClick { get; set; }
    [Parameter] public EventCallback<string> OnRename { get; set; }

    private bool _isEditingName;
    private string _editedName = string.Empty;

    private void StartRename()
    {
        _editedName = ListName;
        _isEditingName = true;
    }

    private void CancelRename() => _isEditingName = false;

    private async Task SaveRenameAsync()
    {
        if (!string.IsNullOrWhiteSpace(_editedName) && _editedName != ListName)
        {
            await OnRename.InvokeAsync(_editedName);
        }
            
        _isEditingName = false;
    }
}