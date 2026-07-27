using FlowBoard.Frontend.Domain.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.CreateBoardDialog;

public partial class BoardTemplateComponent
{
    [Parameter] public BoardTemplate Value { get; set; }
    [Parameter] public EventCallback<BoardTemplate> ValueChanged { get; set; }

    private async Task SelectTemplateAsync(BoardTemplate template)
    {
        Value = template;
        await ValueChanged.InvokeAsync(template);
    }

    private string GetIcon(string name) => name switch
    {
        "ViewKanban" => Icons.Material.Filled.ViewKanban,
        "Add" => Icons.Material.Filled.Add,
        "DirectionsRun" => Icons.Material.Filled.DirectionsRun,
        "Timeline" => Icons.Material.Filled.Timeline,
        _ => Icons.Material.Filled.Dashboard
    };
}