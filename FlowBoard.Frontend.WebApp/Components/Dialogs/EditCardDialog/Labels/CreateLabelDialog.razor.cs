using Microsoft.AspNetCore.Components;
using MudBlazor;
using FlowBoard.Frontend.Domain.Constants;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.Labels;

public partial class CreateLabelDialog
{
    [CascadingParameter] public IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public bool IsEdit { get; set; }
    [Parameter] public string InitialName { get; set; } = string.Empty;
    [Parameter] public string InitialColor { get; set; } = LabelColors.Default;

    private string _name = string.Empty;
    private string _color = LabelColors.Default;

    protected override void OnInitialized()
    {
        _name = InitialName;
        _color = string.IsNullOrWhiteSpace(InitialColor) 
            ? LabelColors.Default : InitialColor;
    }

    private void Submit()
    {
        if (string.IsNullOrWhiteSpace(_name))
        {
            return;
        }

        MudDialog.Close(DialogResult.Ok((_name, _color)));
    }

    private void Cancel() => MudDialog.Cancel();

    private string GetSwatchStyle(string color)
    {
        var border = _color == color
            ? "3px solid #172b4d"
            : "2px solid transparent";

        return $"width: 36px; height: 36px; border-radius: 8px; " +
               $"background-color: {color}; cursor: pointer; border: {border};";
    }
}