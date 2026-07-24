using MudBlazor;
using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.WebApp.Components.Dialogs.InviteBoardDialog;
using FlowBoard.Frontend.Domain.Authorization;

namespace FlowBoard.Frontend.WebApp.Pages.BoardDetails;

public partial class BoardDetails
{
    private bool _isOwnerPro => _board?.OwnerIsPro ?? false;
        
    private async Task OpenInviteDialogAsync()
    {
        var assignable = BoardPermissions.AssignableRoles(_board!.UserRole);

        var parameters = new DialogParameters<InviteMemberDialog>
        {
            { x => x.AvailableRoles, [.. assignable] },
            { x => x.CurrentMembersCount, _board.Members.Count },
            { x => x.IsOwnerPro, _isOwnerPro },
            { x => x.BoardId, _board.Id },
            { x => x.IsPublic, _board.IsPublic }
        };

        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            CloseButton = true
        };

        var dialog = await DialogService.ShowAsync<InviteMemberDialog>(
            "Invite Member", parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled && result.Data is InviteMemberDto inviteDto)
        {
            await HandleInviteMemberAsync(inviteDto);
        }
    }

    private async Task HandleInviteMemberAsync(InviteMemberDto inviteDto)
    {
        var result = await BoardService.InviteMemberAsync(Id, inviteDto);

        ShowResult(result.Success,
            $"User {inviteDto.Email} successfully invited!",
            result.Error ?? "Failed");
    }
}