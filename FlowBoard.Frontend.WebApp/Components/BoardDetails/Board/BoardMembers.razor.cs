using Microsoft.AspNetCore.Components;
using MudBlazor;
using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Domain.Enums;
using FlowBoard.Frontend.Services.Abstractions;

namespace FlowBoard.Frontend.WebApp.Components.BoardDetails.Board;

public partial class BoardMembers
{
    [Parameter] public List<BoardMemberDto> Members { get; set; } = [];
    [Parameter] public Guid BoardId { get; set; }
    [Parameter] public Guid CurrentUserId { get; set; }
    [Parameter] public BoardRole CurrentUserRole { get; set; }
    [Parameter] public EventCallback OnMembersChanged { get; set; }
    [Parameter] public Guid CreatedBy { get; set; }

    [Inject] private IBoardService BoardService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    private bool _isOpen;

    private void ToggleOpen() => _isOpen = !_isOpen;
    private void Close() => _isOpen = false;

    private bool CanRemove(BoardMemberDto member)
        => CurrentUserRole == BoardRole.Owner
        && member.UserId != CreatedBy;

    private bool CanLeave(BoardMemberDto member)
        => CurrentUserRole != BoardRole.Owner
        && member.UserId == CurrentUserId;

    private async Task RemoveMemberAsync(Guid userId)
    {
        var result = await BoardService.RemoveMemberAsync(
            BoardId, userId);

        if (!result.Success)
        {
            Snackbar.Add(result.Error ?? "Failed", Severity.Error);
            return;
        }

        Snackbar.Add("Member removed", Severity.Success);
        await OnMembersChanged.InvokeAsync();
    }

    private async Task LeaveBoardAsync()
    {
        var result = await BoardService.LeaveBoardAsync(BoardId);

        if (!result.Success)
        {
            Snackbar.Add(result.Error ?? "Failed", Severity.Error);
            return;
        }

        Snackbar.Add("You left the board", Severity.Success);
        await OnMembersChanged.InvokeAsync();
    }

    private static Color GetRoleColor(BoardRole role) => role switch
    {
        BoardRole.Owner => Color.Warning,
        BoardRole.Member => Color.Primary,
        BoardRole.Viewer => Color.Default,
        _ => Color.Default
    };
}