using Microsoft.JSInterop;
using MudBlazor;
using FlowBoard.Frontend.Domain.DTOs.Lists;
using FlowBoard.Frontend.Domain.DTOs.Cards;

namespace FlowBoard.Frontend.WebApp.Pages.BoardDetails;

public partial class BoardDetails
{
    [JSInvokable]
    public async Task HandleListMovedJS(string listId, int newIndex)
    {
        var listGuid = Guid.Parse(listId);

        MoveListLocally(listGuid, newIndex);
        StateHasChanged();

        var dto = new MoveListDto(NewPosition: newIndex);
        var success = await ListService.MoveAsync(Id, listGuid, dto);


        if (!success)
        {
            Snackbar.Add("Failed to save list position", Severity.Error);
            await RefreshBoardAsync();
            await InvokeAsync(StateHasChanged);
        }
    }

    [JSInvokable]
    public async Task HandleCardMovedJS(string cardId, string toListId, int newIndex)
    {
        var cardGuid = Guid.Parse(cardId);
        var toListGuid = Guid.Parse(toListId);

        MoveCardLocally(cardGuid, toListGuid, newIndex);
        StateHasChanged();

        var dto = new MoveCardDto(
            NewListId: toListGuid,
            NewPosition: newIndex);

        var success = await CardService.MoveAsync(Id, cardGuid, dto);

        if (!success)
        {
            Snackbar.Add("Failed to save card position", Severity.Error);
            await RefreshBoardAsync();
            await InvokeAsync(StateHasChanged);
        }
    }

    private void MoveCardLocally(Guid cardId, Guid toListId, int newIndex)
    {
        if (_board is null)
        {
            return;
        } 
        
        var card = _board.Lists
            .SelectMany(l => l.Cards ?? [])
            .FirstOrDefault(c => c.Id == cardId);

        if (card is null)
        {
            return;

        }
        
        var sourceList = _board.Lists.FirstOrDefault(l => l.Cards?.Contains(card) == true);
        sourceList?.Cards?.Remove(card);

        var targetList = _board.Lists.FirstOrDefault(l => l.Id == toListId);
        if (targetList?.Cards is not null)
        {
            var clampedIndex = Math.Clamp(newIndex, 0, targetList.Cards.Count);
            targetList.Cards.Insert(clampedIndex, card);

            for (int i = 0; i < targetList.Cards.Count; i++)
            {
                targetList.Cards[i] = targetList.Cards[i] with { Position = i };
            }
        }
    }

    private void MoveListLocally(Guid listId, int newIndex)
    {
        if (_board is null)
        {
            return;
        }

        var list = _board.Lists.FirstOrDefault(l => l.Id == listId);
        if (list is null)
        {
            return;
        }
        
        _board.Lists.Remove(list);

        var clampedIndex = Math.Clamp(newIndex, 0, _board.Lists.Count);
        _board.Lists.Insert(clampedIndex, list);

        for (int i = 0; i < _board.Lists.Count; i++)
        {
            _board.Lists[i] = _board.Lists[i] with { Position = i };
        }
    }
}
