using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.Comments;

public partial class CommentComposer
{
    [Parameter] public EventCallback<string> OnSubmit { get; set; }

    private bool _isWriting;
    private string _message = string.Empty;

    private void Cancel()
    {
        _message = string.Empty;
        _isWriting = false;
    }

    private async Task SubmitAsync()
    {
        if (string.IsNullOrWhiteSpace(_message))
        {
            return;
        }

        await OnSubmit.InvokeAsync(_message);
        Cancel();
    }
}