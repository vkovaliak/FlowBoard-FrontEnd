using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Domain.Enums;
using FlowBoard.Frontend.Domain.Models.Boards;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.InviteBoardDialog;

public partial class InviteMemberDialog
{
    [CascadingParameter] public IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public List<BoardRole> AvailableRoles { get; set; } = [];

    private readonly InviteMemberModel _model = new();
    private BoardRole _role;

    protected override void OnInitialized()
    {
        _role = AvailableRoles.Contains(BoardRole.Member)
            ? BoardRole.Member
            : AvailableRoles.FirstOrDefault();
    }

    private void Submit()
    {
        if (string.IsNullOrWhiteSpace(_model.Email))
        {
            return;
        }

        var dto = new InviteMemberDto(_model.Email, _role);
        MudDialog.Close(DialogResult.Ok(dto));
    }

    private void Cancel() => MudDialog.Cancel();

    private static string GetRoleIcon(BoardRole role) => role switch
    {
        BoardRole.Admin => Icons.Material.Filled.AdminPanelSettings,
        BoardRole.Member => Icons.Material.Filled.Edit,
        BoardRole.Viewer => Icons.Material.Filled.Visibility,
        _ => Icons.Material.Filled.Person
    };

    private static string GetRoleDescription(BoardRole role) => role switch
    {
        BoardRole.Admin => "Can manage members and edit the board",
        BoardRole.Member => "Can view and edit the board",
        BoardRole.Viewer => "Can only view the board",
        _ => string.Empty
    };
}