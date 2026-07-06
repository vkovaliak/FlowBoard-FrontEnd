using FlowBoard.Frontend.Domain.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.ChangeRoleDialog;

public partial class ChangeRoleDialog
{
    [CascadingParameter] public IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public string MemberName { get; set; } = string.Empty;
    [Parameter] public BoardRole CurrentRole { get; set; }
    [Parameter] public List<BoardRole> AvailableRoles { get; set; } = [];

    private BoardRole _selectedRole;

    protected override void OnInitialized()
    {
        _selectedRole = AvailableRoles.Contains(CurrentRole)
            ? CurrentRole
            : AvailableRoles.FirstOrDefault();
    }

    private void Submit() 
        => MudDialog.Close(DialogResult.Ok(_selectedRole));
        
    private void Cancel() => MudDialog.Cancel();
}