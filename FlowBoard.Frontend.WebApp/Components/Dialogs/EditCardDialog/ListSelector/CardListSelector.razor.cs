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
        var result = await CardService.MoveAsync(BoardId, CardId, dto);

        if (!result.Success)
        {
            Snackbar.Add(result.Error ?? "Failed", Severity.Error);
        }
    }
}