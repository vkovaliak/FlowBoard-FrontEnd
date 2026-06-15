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
        var dto = new MoveListDto(NewPosition: newIndex);
        var success = await ListService.MoveAsync(Id, Guid.Parse(listId), dto);

        if (!success)
        {
            Snackbar.Add("Failed to save list position", Severity.Error);
        }

        await RefreshBoardAsync();
    }

    [JSInvokable]
    public async Task HandleCardMovedJS(string cardId, string toListId, int newIndex)
    {
        var dto = new MoveCardDto(
            NewListId: Guid.Parse(toListId),
            NewPosition: newIndex);

        var success = await CardService.MoveAsync(Id, Guid.Parse(cardId), dto);

        if (!success)
        {
            Snackbar.Add("Failed to save card position", Severity.Error);
        }
            
        await RefreshBoardAsync();
    }
}