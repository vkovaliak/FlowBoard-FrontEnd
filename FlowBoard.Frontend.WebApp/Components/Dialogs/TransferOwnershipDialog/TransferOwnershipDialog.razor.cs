using FlowBoard.Frontend.Domain.DTOs.Boards;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.TransferOwnershipDialog;

public partial class TransferOwnershipDialog
{
    [CascadingParameter] public IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public List<BoardMemberDto> Candidates { get; set; } = [];

    private Guid _selectedId = Guid.Empty;

    private void Select(Guid userId) => _selectedId = userId;
    private bool IsSelected(Guid userId) => _selectedId == userId;

    private void Submit()
    {
        if (_selectedId == Guid.Empty)
        {
            return;
        }

        MudDialog.Close(DialogResult.Ok(_selectedId));
    }

    private void Cancel() => MudDialog.Cancel();
}