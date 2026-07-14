using FlowBoard.Frontend.Domain.DTOs.Cards;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.BoardDetails.Calendar;

public partial class BoardCalendarMonth
{
    [Parameter] public DateTime CurrentMonth { get; set; }
    [Parameter] public List<CardDto> Cards { get; set; } = [];
    [Parameter] public EventCallback<CardDto> OnCardClick { get; set; }

    private readonly string[] _dayNames =
        ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];

    private List<DateTime> _days = [];

    protected override void OnParametersSet()
        => BuildDays();

    private void BuildDays()
    {
        _days = [];

        var firstOfMonth = new DateTime(
            CurrentMonth.Year, CurrentMonth.Month, 1);

        var startOffset = (int)firstOfMonth.DayOfWeek;
        var gridStart = firstOfMonth.AddDays(-startOffset);

        for (var i = 0; i < 35; i++)
        {
            _days.Add(gridStart.AddDays(i));
        }
    }

    private List<CardDto> GetCardsForDay(DateTime date)
        => Cards
            .Where(c => c.DueDate.HasValue
                && c.DueDate.Value.Date == date.Date)
            .ToList();
}