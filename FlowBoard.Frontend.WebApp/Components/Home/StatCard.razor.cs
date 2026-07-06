using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.Home;

public partial class StatCard
{
    [Parameter, EditorRequired] public string Icon { get; set; } = default!;
    [Parameter, EditorRequired] public string Value { get; set; } = default!;
    [Parameter, EditorRequired] public string Label { get; set; } = default!;

    [Parameter] public string IconBg { get; set; } = "#eef2ff";
    [Parameter] public string IconColor { get; set; } = "#6366f1";
}