using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Domain.DTOs.Cards;
using FlowBoard.Frontend.Domain.Models.Boards;
using FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.BoardDetails.Timeline;

public partial class TimelineView
{
    [Inject] public IDialogService DialogService { get; set; } = default!;

    [Parameter] public BoardDetailsDto Board { get; set; } = default!;

    [Parameter] public EventCallback<(
        Guid ListId, Guid CardId, UpdateCardDto Dto)> OnCardUpdated { get; set; }

    private const int DayWidth = 22;
    private const int ColumnWidth = DayWidth * 7;

    private List<TimelineRowModel> _rows = [];
    private List<DateTime> _columns = [];
    private DateTime _rangeStart;
    private DateTime _rangeEnd;
    private double _totalWidth;
    private double _todayOffset = -1;

    protected override void OnParametersSet()
    {
        _rows = Board.Lists
            .SelectMany(list => (list.Cards ?? [])
                .Where(c => c.StartTime.HasValue && c.DueDate.HasValue)
                .Select(card => new TimelineRowModel
                {
                    Card = card, ListId = list.Id
                }))
            .OrderBy(r => r.Card.StartTime)
            .ToList();

        if (_rows.Count == 0)
        {
            return;
        }

        BuildRange();
    }

    private void BuildRange()
    {
        var minStart = _rows.Min(r => r.Card.StartTime!.Value).Date;
        var maxEnd = _rows.Max(r => r.Card.DueDate!.Value).Date;

        _rangeStart = StartOfWeek(minStart).AddDays(-7);
        _rangeEnd = StartOfWeek(maxEnd).AddDays(14);

        _columns = [];
        for (var d = _rangeStart; d <= _rangeEnd; d = d.AddDays(7))
        {
            _columns.Add(d);
        }

        _totalWidth = _columns.Count * ColumnWidth;

        var today = DateTime.Today;
        _todayOffset = today >= _rangeStart && today <= _rangeEnd
            ? (today - _rangeStart).TotalDays * DayWidth
            : -1;
    }

    private string GetBarStyle(CardDto card)
    {
        var start = card.StartTime!.Value.Date;
        var end = card.DueDate!.Value.Date;

        var offsetDays = (start - _rangeStart).TotalDays;
        var durationDays = (end - start).TotalDays + 1;

        var left = offsetDays * DayWidth;
        var width = durationDays * DayWidth;

        var color = card.Labels.FirstOrDefault()?.Color ?? "#3B82F6";

        return $"left: {left}px; width: {width}px; background-color: {color};";
    }

    private async Task OpenCardAsync(TimelineRowModel row)
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

    private static DateTime StartOfWeek(DateTime date)
    {
        int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
        return date.AddDays(-diff).Date;
    }
}