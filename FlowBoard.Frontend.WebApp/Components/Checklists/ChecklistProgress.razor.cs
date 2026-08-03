using FlowBoard.Frontend.Domain.DTOs.Checklists;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.Checklists;

public partial class ChecklistProgress
{
    [Parameter] public List<ChecklistItemDto> Items { get; set; } = [];
    [Parameter] public Size IconSize { get; set; } = Size.Small;
    [Parameter] public string Class { get; set; } = string.Empty;

    [Parameter] public bool OnCover { get; set; }

    private int Total => Items.Count;
    private int Done => Items.Count(x => x.IsCompleted);

    private double Percentage
        => Total == 0 ? 0 : (double)Done / Total * 100;

    private bool IsCompleted => Total > 0 && Done == Total;

    private string TextColor => OnCover ? "#ffffff" : "#64748B";

    private string CheckColor => OnCover ? "#ffffff" : "#2563EB";

    private string TextShadow => OnCover
        ? "text-shadow: 0 1px 2px rgba(0,0,0,0.4);"
        : "";
}