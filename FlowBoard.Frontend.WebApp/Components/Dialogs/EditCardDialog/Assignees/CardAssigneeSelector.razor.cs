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
    [Inject] public IDialogService DialogService { get; set; } = default!;

    [Parameter] public Guid BoardId { get; set; }
    [Parameter] public Guid CardId { get; set; }
    [Parameter] public List<CardAssigneeDto> Assignees { get; set; } = [];
    [Parameter] public List<BoardMemberDto> BoardMembers { get; set; } = [];

    private async Task OpenAssignDialogAsync()
    {
        var parameters = new DialogParameters<AssignMembersDialog>
        {
            { x => x.BoardMembers, BoardMembers },
            { x => x.AssignedUserIds, Assignees.Select(
                a => a.UserId).ToHashSet() }
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.ExtraSmall,
            FullWidth = true,
            CloseButton = false
        };

        var dialog = await DialogService.ShowAsync<AssignMembersDialog>(
            "Assign members", parameters, options);

        var result = await dialog.Result;

        if (result is null || result.Canceled)
        {
            return;
        }

        if (result.Data is not HashSet<Guid> selectedIds)
        {
            return;
        }

        await ApplyChangesAsync(selectedIds);
    }

    private async Task ApplyChangesAsync(HashSet<Guid> selectedIds)
    {
        var currentIds = Assignees.Select(a => a.UserId).ToHashSet();

        foreach (var userId in selectedIds.Except(currentIds))
        {
            var result = await CardService.AssignMemberAsync(
                BoardId, CardId, userId);
            if (!result.Success)
            {
                Snackbar.Add(result.Error ?? "Failed", Severity.Error);
            }
        }

        foreach (var userId in currentIds.Except(selectedIds))
        {
            var result = await CardService.UnassignMemberAsync(
                BoardId, CardId, userId);

            if (!result.Success)
            {
                Snackbar.Add(result.Error ?? "Failed", Severity.Error);
            }
        }
    }
}