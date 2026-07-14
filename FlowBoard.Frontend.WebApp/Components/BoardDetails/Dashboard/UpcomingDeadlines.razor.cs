using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Domain.DTOs.Cards;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.BoardDetails.Dashboard;

public partial class UpcomingDeadlines
{
    [Parameter] public BoardDetailsDto Board { get; set; } = default!;
    [Parameter] public EventCallback<CardDto> OnCardClick { get; set; }

    private List<CardDto> _deadlines = [];

    protected override void OnParametersSet()
    {
        _deadlines = Board.Lists
            .SelectMany(l => l.Cards ?? [])
            .Where(c => !c.IsCompleted && c.DueDate.HasValue
                && c.DueDate.Value.Date >= DateTime.Today)
            .OrderBy(c => c.DueDate)
            .Take(6)
            .ToList();
    }

    private Color GetColor(CardDto card)
    {
        if (card.DueDate!.Value.Date == DateTime.Today)
        {
            return Color.Error;
        }

        if (card.DueDate.Value.Date <= DateTime.Today.AddDays(3))
        {
            return Color.Warning;
        }

        return Color.Default;
    }
}