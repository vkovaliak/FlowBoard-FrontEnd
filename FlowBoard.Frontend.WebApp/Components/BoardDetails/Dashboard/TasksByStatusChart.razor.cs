using FlowBoard.Frontend.Domain.DTOs.Boards;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.BoardDetails.Dashboard;

public partial class TasksByStatusChart
{
    [Parameter] public BoardDetailsDto Board { get; set; } = default!;

    private double[] _data = [];
    private string[] _labels = [];
    private ChartOptions _chartOptions = new();

    private int _total;

    private static readonly string[] Palette =
        ["#6366f1", "#f59e0b", "#10b981", 
         "#ef4444", "#8b5cf6", "#06b6d4"];
        
    protected override void OnInitialized()
    {
        _chartOptions = new ChartOptions
        {
            ChartPalette = Palette
        };
    }

    protected override void OnParametersSet()
    {
        var lists = Board.Lists
            .OrderBy(l => l.Position)
            .Where(l => (l.Cards?.Count ?? 0) > 0)
            .ToList();

        _labels = lists.Select(l => l.Name)
                       .ToArray();

        _data = lists.Select(
                    l => (double)(l.Cards?.Count ?? 0))
                    .ToArray();

        _total = (int)_data.Sum();
    }

    private string GetColor(int index) 
        => Palette[index % Palette.Length];
}