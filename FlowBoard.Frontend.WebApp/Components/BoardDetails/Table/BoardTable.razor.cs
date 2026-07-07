using Microsoft.AspNetCore.Components;
using MudBlazor;
using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Domain.DTOs.Cards;
using FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog;

namespace FlowBoard.Frontend.WebApp.Components.BoardDetails.Table;

public partial class BoardTable
{
    [Inject] public IDialogService DialogService { get; set; } = default!;

    [Parameter] public BoardDetailsDto Board { get; set; } = default!;

    [Parameter] public EventCallback<(
        Guid ListId, Guid CardId, UpdateCardDto Dto)> OnCardUpdated { get; set; }

    public record TableRow(CardDto Card, Guid ListId, string ListName);

    private IEnumerable<TableRow> GetRows()
    {
        return Board.Lists
            .OrderBy(l => l.Position)
            .SelectMany(list => (list.Cards ?? [])
                .OrderBy(c => c.Position)
                .Select(card => new TableRow(card, list.Id, list.Name)));
    }

    private async Task OpenCardAsync(TableRow row)
    {
        var parameters = new DialogParameters<EditCardDialog>
        {
            { x => x.BoardId, Board.Id },
            { x => x.CardId, row.Card.Id }
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

        var result = await dialog.Result;

        if (!result!.Canceled && result.Data is UpdateCardDto updateDto)
        {
            await OnCardUpdated.InvokeAsync(
                (row.ListId, row.Card.Id, updateDto));
        }
    }

    private static string GetDueClass(CardDto card)
    {
        if (card.IsCompleted) return "tbl-due-done";
        if (card.DueDate < DateTime.Today) return "tbl-due-overdue";
        if (card.DueDate == DateTime.Today) return "tbl-due-today";
        return "";
    }
}