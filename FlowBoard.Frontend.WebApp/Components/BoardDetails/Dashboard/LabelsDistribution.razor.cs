using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Domain.Models.Boards;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.BoardDetails.Dashboard;

public partial class LabelsDistribution
{
    [Parameter] public BoardDetailsDto Board { get; set; } = default!;

    private List<LabelStatModel> _labels = [];
    private int _max = 1;

    protected override void OnParametersSet()
    {
        _labels = Board.Lists
            .SelectMany(l => l.Cards ?? [])
            .SelectMany(c => c.Labels)
            .GroupBy(l => new { l.Name, l.Color })
            .Select(g => new LabelStatModel{
                Name = g.Key.Name, 
                Color = g.Key.Color, 
                Count = g.Count()
            })
            .OrderByDescending(l => l.Count)
            .ToList();

        _max = _labels.Count > 0 ? _labels.Max(l => l.Count) : 1;
    }

    private double GetPercent(int count) 
        => (double)count / _max * 100;
}