using FlowBoard.Frontend.Domain.DTOs.Cards;
using FlowBoard.Frontend.Services.State;
using FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Pages.BoardDetails;

public partial class BoardDetails
{
    [Inject] public TasksState TasksState { get; set; } = default!;
    [Inject] private IJSRuntime JS { get; set; } = default!;

    private Guid? _openedCardId;

    private async Task HandleCreateCardAsync(CreateCardDto dto)
    {
        if (_board == null)
        {
            return;
        }
        var result = await CardService.CreateAsync(_board.Id, dto);

        ShowResult(result.Success, "Card added!", 
            result.Error ?? "Failed");
    }

    private async Task HandleUpdateCardAsync(
        (Guid ListId, Guid CardId, UpdateCardDto Dto) args)
    {
        var result = await CardService.UpdateAsync(
            Id, args.ListId, args.CardId, args.Dto);

        ShowResult(result.Success,
            "Card updated successfully!",
            result.Error ?? "Failed");
    }

    private async Task HandleTableCardUpdatedAsync(
        (Guid ListId, Guid CardId, UpdateCardDto Dto) args)
    {
        var result = await CardService.UpdateAsync(
            Id, args.ListId, args.CardId, args.Dto);

        ShowResult(result.Success,
            "Card updated successfully!",
            result.Error ?? "Failed");
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

        var result = await CardService.DeleteAsync(
            Id, args.ListId, args.CardId);

        ShowResult(result.Success,
            $"Card '{args.CardName}' deleted",
            result.Error ?? "Failed");
    }

    private async Task HandleToggleCardCompleteAsync((CardDto Card, MouseEventArgs Args) args)
    {
        var card = args.Card;
        var eventArgs = args.Args;

        var wasCompleted = card.IsCompleted;
        var result = await CardService.ToggleCompletionAsync(Id, card.Id);

        if (!result.Success)
        {
            Snackbar.Add(result.Error ?? "Failed", Severity.Error);
        }

        if (!wasCompleted)
        {
            try
            {
                await JS.InvokeVoidAsync(
                    "flowConfetti.burstAt", eventArgs.ClientX, eventArgs.ClientY);
            }
            catch
            {
            }
        }
        TasksState.NotifyChanged();
    }

    private async Task HandleDuplicateCardAsync(CardDto card)
    {
        if (_board == null)
        {
            return;
        }

        var result = await CardService.DuplicateAsync(_board.Id, card.Id);

        ShowResult(result.Success,
            "Card duplicated!",
            result.Error ?? "Failed");
    }

    private async Task TryOpenCardFromQueryAsync()
    {
        var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);

        var cardId = GetCardIdFromQuery(uri.Query);

        if (cardId is null)
        {
            return;
        }

        if (_openedCardId == cardId)
        {
            return;
        }

        var card = _board?.Lists
            .SelectMany(l => l.Cards ?? [])
            .FirstOrDefault(c => c.Id == cardId);

        if (card is null)
        {
            ClearCardQuery();
            return;
        }

        _openedCardId = cardId;
        await OpenCardDialogAsync(card);
    }

    private static Guid? GetCardIdFromQuery(string queryString)
    {
        if (string.IsNullOrEmpty(queryString))
        {
            return null;
        }

        var parsed = System.Web.HttpUtility.ParseQueryString(queryString);
        var raw = parsed["card"];

        return Guid.TryParse(raw, out var id) ? id : null;
    }

    private async Task OpenCardDialogAsync(CardDto card)
    {
        var parameters = new DialogParameters<EditCardDialog>
        {
            { x => x.BoardId, Id },
            { x => x.CardId, card.Id }
        };

        var options = new DialogOptions
        {
            Position = DialogPosition.CenterRight,
            MaxWidth = MaxWidth.False,
            FullWidth = false,
            CloseButton = false,
            NoHeader = false
        };

        var dialog = await DialogService.ShowAsync<EditCardDialog>(
            null, parameters, options);

        await dialog.Result;

        _openedCardId = null;
        ClearCardQuery();
    }

    private void ClearCardQuery()
    {
        NavigationManager.NavigateTo($"/boards/{Id}", replace: true);
    }
}