using Microsoft.AspNetCore.Components;
using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Domain.DTOs.Cards;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.Assignees;

public partial class CardAssigneeSelector
{
    [Inject]
    public ICardService CardService { get; set; } = default!;

    [Parameter]
    public Guid BoardId { get; set; }

    [Parameter]
    public Guid CardId { get; set; }

    [Parameter]
    public List<CardAssigneeDto> Assignees { get; set; } = [];

    [Parameter]
    public List<BoardMemberDto> BoardMembers { get; set; } = [];

    private Guid? _selectedUserId = null;

    private IEnumerable<BoardMemberDto> AvailableMembers =>
        BoardMembers.Where(x =>
            Assignees.All(a => a.UserId != x.UserId));

    private async Task AssignUserAsync(Guid? userId)
    {
        if (userId is null)
        {
            return;
        }
           
        var success = await CardService.AssignMemberAsync(
            BoardId, CardId, userId.Value);

        if (success)
        {
            var member = BoardMembers.First(x => x.UserId == userId);

            Assignees.Add(new CardAssigneeDto(
                member.UserId,
                member.EmailAddress));

            _selectedUserId = null;

            StateHasChanged();
        }
    }

    private async Task RemoveAssigneeAsync(
        CardAssigneeDto assignee)
    {
        var success = await CardService.UnassignMemberAsync(
            BoardId, CardId, assignee.UserId);

        if (success)
        {
            Assignees.RemoveAll(
                x => x.UserId == assignee.UserId);

            StateHasChanged();
        }
    }
}