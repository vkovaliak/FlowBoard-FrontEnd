using FlowBoard.Frontend.Domain.DTOs.Labels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.Labels;

public partial class LabelChip
{
    [Parameter] public LabelDto Label { get; set; } = default!;
    [Parameter] public Size Size { get; set; } = Size.Small;
    [Parameter] public EventCallback OnClose { get; set; }

    private string GetStyle()
    {
        var style = $"background-color: {Label.Color}; color: white; " +
                    "text-transform: none; font-weight: 500;";

        return style;
    }
}