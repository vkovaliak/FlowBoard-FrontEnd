using FlowBoard.Frontend.Domain.DTOs.Checklists;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.Checklists;

public partial class ChecklistProgress
{
    [Parameter] public List<ChecklistItemDto> Items { get; set; } = [];
    [Parameter] public Size IconSize { get; set; } = Size.Small;
    [Parameter] public string Class { get; set; } = string.Empty;

    private int Total => Items.Count;
    private int Done => Items.Count(x => x.IsCompleted);

    private double Percentage
        => Total == 0 ? 0 : (double)Done / Total * 100;

    private bool IsCompleted => Total > 0 && Done == Total;
}