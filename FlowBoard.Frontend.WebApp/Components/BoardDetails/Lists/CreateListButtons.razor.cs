using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.BoardDetails.Lists;

public partial class CreateListButtons
{
    [Parameter] 
    public Guid BoardId { get; set; }

    [Parameter] 
    public EventCallback<string> OnListCreate { get; set; }

    private bool _isCreatingList;
    private string _newListName = string.Empty;

    private void ToggleForm()
    {
        _isCreatingList = !_isCreatingList;
    }
    
    private async Task SubmitFormAsync()
    {
        if (string.IsNullOrWhiteSpace(_newListName))
        {
            return;
        } 
        
        await OnListCreate.InvokeAsync(_newListName);
        
        _newListName = string.Empty;
        _isCreatingList = false;
    }
}