using Microsoft.AspNetCore.Components;
using MudBlazor;
using FlowBoard.Frontend.Domain.Authorization;
using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Domain.Enums;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.WebApp.Components.Dialogs.ChangeRoleDialog;
using FlowBoard.Frontend.WebApp.Components.Dialogs.TransferOwnershipDialog;

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
    [Inject] private IDialogService DialogService { get; set; } = default!;

    private bool _isOpen;

    private void ToggleOpen() => _isOpen = !_isOpen;
    private void Close() => _isOpen = false;

    private bool CanManageRoles
        => BoardPermissions.CanManageRoles(CurrentUserRole);

    private bool CanTransfer
        => BoardPermissions.CanTransferOwnership(CurrentUserRole);

    private bool CanRemove(BoardMemberDto member)
    {
        if (member.UserId == CurrentUserId)
        {
            return false;
        }

        return BoardPermissions.CanRemoveMembers(CurrentUserRole)
            && BoardPermissions.CanModifyMember(CurrentUserRole, member.Role);
    }

    private bool CanChangeRole(BoardMemberDto member)
    {
        if (member.UserId == CurrentUserId)
        {
            return false;
        }

        return CanManageRoles
            && BoardPermissions.CanModifyMember(
                CurrentUserRole, member.Role);
    }

    private bool CanLeave(BoardMemberDto member)
        => member.UserId == CurrentUserId
        && BoardPermissions.CanLeaveBoard(CurrentUserRole);

    private async Task RemoveMemberAsync(Guid userId)
    {
        var confirmed = await DialogService.ShowMessageBoxAsync(
            "Remove Member",
            "Are you sure you want to remove this member from the board?",
            yesText: "Remove", cancelText: "Cancel") == true;

        if (!confirmed)
        {
            return;
        }

        var result = await BoardService.RemoveMemberAsync(BoardId, userId);

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
        var confirmed = await DialogService.ShowMessageBoxAsync(
            "Leave Board",
            "Are you sure you want to leave this board?",
            yesText: "Leave", cancelText: "Cancel") == true;

        if (!confirmed)
        {
            return;
        }

        var result = await BoardService.LeaveBoardAsync(BoardId);

        if (!result.Success)
        {
            Snackbar.Add(result.Error ?? "Failed", Severity.Error);
            return;
        }

        Snackbar.Add("You left the board", Severity.Success);
        await OnMembersChanged.InvokeAsync();
    }

    private async Task OpenChangeRoleDialogAsync(BoardMemberDto member)
    {
        var assignable = BoardPermissions.AssignableRoles(CurrentUserRole);

        var parameters = new DialogParameters<ChangeRoleDialog>
        {
            { x => x.MemberName, member.UserName },
            { x => x.CurrentRole, member.Role },
            { x => x.AvailableRoles, [.. assignable] }
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.ExtraSmall,
            FullWidth = true,
            CloseButton = true
        };

        var dialog = await DialogService.ShowAsync<ChangeRoleDialog>(
            "Change Role", parameters, options);
        var result = await dialog.Result;

        if (result is null || result.Canceled)
        {
            return;
        }

        if (result.Data is not BoardRole newRole || newRole == member.Role)
        {
            return;
        }

        var dto = new ChangeMemberRoleDto(newRole);
        var changeResult = await BoardService.ChangeMemberRoleAsync(
            BoardId, member.UserId, dto);

        if (!changeResult.Success)
        {
            Snackbar.Add(changeResult.Error ?? "Failed", Severity.Error);
            return;
        }

        Snackbar.Add("Role updated", Severity.Success);
        await OnMembersChanged.InvokeAsync();
    }

    private async Task OpenTransferOwnershipDialogAsync()
    {
        var candidates = Members
            .Where(m => m.UserId != CurrentUserId)
            .ToList();

        if (candidates.Count == 0)
        {
            Snackbar.Add(
                "There are no other members to transfer ownership to.",
                Severity.Warning);
            return;
        }

        var parameters = new DialogParameters<TransferOwnershipDialog>
        {
            { x => x.Candidates, candidates }
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.ExtraSmall,
            FullWidth = true,
            CloseButton = true
        };

        var dialog = await DialogService.ShowAsync<TransferOwnershipDialog>(
            "Transfer Ownership", parameters, options);
        var result = await dialog.Result;

        if (result is null || result.Canceled)
        {
            return;
        }

        if (result.Data is not Guid newOwnerId)
        {
            return;
        }

        var dto = new TransferOwnershipDto(newOwnerId);
        var transferResult = await BoardService.TransferOwnershipAsync(
            BoardId, dto);

        if (!transferResult.Success)
        {
            Snackbar.Add(transferResult.Error ?? "Failed", Severity.Error);
            return;
        }

        Snackbar.Add(
            "Ownership transferred. You are now an admin.",
            Severity.Success);
        await OnMembersChanged.InvokeAsync();
    }

    private static Color GetRoleColor(BoardRole role) => role switch
    {
        BoardRole.Owner => Color.Warning,
        BoardRole.Admin => Color.Primary,
        BoardRole.Member => Color.Tertiary,
        BoardRole.Viewer => Color.Default,
        _ => Color.Default
    };
}