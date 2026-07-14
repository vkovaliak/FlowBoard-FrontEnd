using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Domain.DTOs.Cards;
using FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.BoardDetails.Calendar;

public partial class BoardCalendarView
{
    [Inject] private IDialogService DialogService { get; set; } = default!;

    [Parameter] public BoardDetailsDto Board { get; set; } = default!;

    [Parameter] public EventCallback<(
        Guid ListId, Guid CardId, UpdateCardDto Dto)> OnCardUpdated { get; set; }

    private DateTime _currentMonth = DateTime.Today;

    private List<CardDto> AllCards =>
        Board.Lists
            .SelectMany(l => l.Cards ?? [])
            .Where(c => c.DueDate.HasValue)
            .ToList();

    private void GoToday() => _currentMonth = DateTime.Today;

    private void PreviousMonth() 
        => _currentMonth = _currentMonth.AddMonths(-1);

    private void NextMonth() 
        => _currentMonth = _currentMonth.AddMonths(1);

    private async Task OpenCardAsync(CardDto card)
    {
        var listId = Board.Lists
            .FirstOrDefault(l => l.Cards?
            .Any(c => c.Id == card.Id) == true)?.Id;

        if (listId is null)
        {
            return;
        }

        var parameters = new DialogParameters<EditCardDialog>
        {
            { x => x.BoardId, Board.Id },
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

        var result = await dialog.Result;

        if (!result!.Canceled && result.Data is UpdateCardDto updateDto)
        {
            await OnCardUpdated.InvokeAsync(
                (listId.Value, card.Id, updateDto));
        }
    }
}