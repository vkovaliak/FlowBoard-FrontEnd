using MudBlazor;
using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.WebApp.Components.Dialogs.InviteBoardDialog;

namespace FlowBoard.Frontend.WebApp.Pages.BoardDetails;

public partial class BoardDetails
{
    private async Task OpenInviteDialogAsync()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        var dialog = await DialogService.ShowAsync<InviteMemberDialog>(
            "Invite Member", options);
        var result = await dialog.Result;

        if (!result!.Canceled && result.Data is InviteMemberDto inviteDto)
        {
            await HandleInviteMemberAsync(inviteDto);
        }
    }

    private async Task HandleInviteMemberAsync(InviteMemberDto inviteDto)
    {
        var success = await BoardService.InviteMemberAsync(Id, inviteDto);

        ShowResult(success,
            $"User {inviteDto.Email} successfully invited!",
            "Failed to invite user.");
    }
}