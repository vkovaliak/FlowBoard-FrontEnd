using Microsoft.AspNetCore.Components;
using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Domain.DTOs.Cards;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.Assignees;

public partial class CardAssigneeSelector
{
    [Inject] public ICardService CardService { get; set; } = default!;
    [Inject] public ISnackbar Snackbar { get; set; } = default!;

    [Parameter] public Guid BoardId { get; set; }
    [Parameter] public Guid CardId { get; set; }
    [Parameter] public List<CardAssigneeDto> Assignees { get; set; } = [];
    [Parameter] public List<BoardMemberDto> BoardMembers { get; set; } = [];

    private Guid? _selectedUserId = null;

    private IEnumerable<BoardMemberDto> AvailableMembers =>
        BoardMembers.Where(x => Assignees.All(a => a.UserId != x.UserId));

    private async Task AssignUserAsync(Guid? userId)
    {
        if (userId is null)
        {
            return;
        }

        var success = await CardService.AssignMemberAsync(
            BoardId, CardId, userId.Value);

        if (!success)
        {
            Snackbar.Add("Failed to assign member", Severity.Error);
            return;
        }

        _selectedUserId = null;
    }

    private async Task RemoveAssigneeAsync(CardAssigneeDto assignee)
    {
        var success = await CardService.UnassignMemberAsync(
            BoardId, CardId, assignee.UserId);

        if (!success)
        {
            Snackbar.Add("Failed to remove member", Severity.Error);
        }
    }
}