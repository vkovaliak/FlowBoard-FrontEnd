using Microsoft.AspNetCore.Components;
using MudBlazor;
using FlowBoard.Frontend.Domain.DTOs.Boards;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.Assignees;

public partial class AssignMembersDialog
{
    [CascadingParameter] public IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public List<BoardMemberDto> BoardMembers { get; set; } = [];
    [Parameter] public HashSet<Guid> AssignedUserIds { get; set; } = [];

    private HashSet<Guid> _selected = [];

    protected override void OnInitialized()
    {
        _selected = [.. AssignedUserIds];
    }

    private void Toggle(Guid userId)
    {
        if (!_selected.Add(userId))
        {
            _selected.Remove(userId);
        }
    }

    private void Apply() => MudDialog.Close(DialogResult.Ok(_selected));
    private void Cancel() => MudDialog.Cancel();
}