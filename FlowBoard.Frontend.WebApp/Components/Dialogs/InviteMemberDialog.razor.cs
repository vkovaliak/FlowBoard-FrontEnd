using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs;

public partial class InviteMemberDialog
{
    [CascadingParameter] 
    IMudDialogInstance MudDialog { get; set; } = default!;

    private string _email = string.Empty;

    private void Submit()
    {
        if (!string.IsNullOrWhiteSpace(_email))
        {
            MudDialog.Close(DialogResult.Ok(_email));
        }
    }

    private void Cancel() 
    {
        MudDialog.Cancel();
    }
}