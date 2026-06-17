using FlowBoard.Frontend.Domain.DTOs.Cards;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Pages.BoardDetails;

public partial class BoardDetails
{
    private async Task HandleCreateCardAsync(CreateCardDto dto)
    {
        if (_board == null)
        {
            return;
        }
        var success = await CardService.CreateAsync(_board.Id, dto);

        ShowResult(success, "Card added!", "Failed to add card");
    }

    private async Task HandleUpdateCardAsync(
        (Guid ListId, Guid CardId, UpdateCardDto Dto) args)
    {
        var success = await CardService.UpdateAsync(
            Id, args.ListId, args.CardId, args.Dto);

        ShowResult(success,
            "Card updated successfully!",
            "Failed to update card");
    }

    private async Task HandleDeleteCardAsync(
        (Guid ListId, Guid CardId, string CardName) args)
    {
        var confirmed = await ConfirmDeleteAsync(
            "Delete Card",
            $"Are you sure you want to delete card '{args.CardName}'?");

        if (!confirmed)
        {
            return;
        }

        var success = await CardService.DeleteAsync(
            Id, args.ListId, args.CardId);

        ShowResult(success,
            $"Card '{args.CardName}' deleted",
            "Failed to delete card");
    }

    private async Task HandleToggleCardCompleteAsync(CardDto card)
    {
        var success = await CardService.ToggleCompletionAsync(Id, card.Id);

        if (!success)
        {
            Snackbar.Add("Failed to update card status", Severity.Error);
        }
    }
}