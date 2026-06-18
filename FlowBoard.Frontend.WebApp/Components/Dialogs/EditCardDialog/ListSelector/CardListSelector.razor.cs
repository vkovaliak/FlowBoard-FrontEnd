using FlowBoard.Frontend.Domain.DTOs.Lists;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Domain.DTOs.Cards;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.ListSelector;

public partial class CardListSelector
{
    [Inject] private ICardService CardService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    [Parameter] public Guid BoardId { get; set; }
    [Parameter] public Guid CardId { get; set; }
    [Parameter] public Guid CurrentListId { get; set; }
    [Parameter] public List<ListDto> Lists { get; set; } = [];

    [Parameter] public EventCallback OnMoved { get; set; }

    private async Task OnListChangedAsync(Guid newListId)
    {
        if (newListId == CurrentListId)
        {
            return;
        }

        var targetList = Lists.FirstOrDefault(l => l.Id == newListId);
        if (targetList is null)
        {
            return;
        }

        var newPosition = targetList.Cards?.Count ?? 0;

        var dto = new MoveCardDto(newListId, newPosition);
        var success = await CardService.MoveAsync(BoardId, CardId, dto);

        if (!success)
        {
            Snackbar.Add("Failed to move card", Severity.Error);
            return;
        }

        CurrentListId = newListId;
        await OnMoved.InvokeAsync();
    }
}