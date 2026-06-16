using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Domain.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs;

public partial class InviteMemberDialog
{
    [CascadingParameter] 
    IMudDialogInstance MudDialog { get; set; } = default!;

    private string _email = string.Empty;
    private BoardRole _role = BoardRole.Member;

    private void Submit()
    {
        if (!string.IsNullOrWhiteSpace(_email))
        {
            MudDialog.Close(DialogResult.Ok(
                new InviteMemberDto(_email, _role)));
        }
    }

    private void Cancel() 
    {
        MudDialog.Cancel();
    }
}