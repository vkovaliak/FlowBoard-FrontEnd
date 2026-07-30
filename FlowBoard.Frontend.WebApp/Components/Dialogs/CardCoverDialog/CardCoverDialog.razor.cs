using FlowBoard.Frontend.Domain.Enums;
using FlowBoard.Frontend.Domain.Models.Cards;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs;

public partial class CardCoverDialog
{
    [CascadingParameter] public IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public string? CurrentColor { get; set; }
    [Parameter] public CardCoverMode CurrentMode { get; set; }

    private string? _selectedColor;
    private CardCoverMode _selectedMode = CardCoverMode.Strip;

    private readonly string[] _presetColors =
    [
        "#4BCE97", 
        "#F5CD47",
        "#FEA362",
        "#F87168",
        "#9F8FEF",
        "#579DFF",
        "#6CC3E0",
        "#94C748",
        "#E774BB",
        "#8590A2",
        "#8C9BAB",
        "#B3BAC5"
    ];

    protected override void OnInitialized()
    {
        _selectedColor = CurrentColor;
        _selectedMode = CurrentMode == CardCoverMode.None
            ? CardCoverMode.Strip
            : CurrentMode;
    }

    private void SelectColor(string color) => _selectedColor = color;

    private void Save()
    {
        var result = new CardCoverResult(_selectedColor, _selectedMode);
        MudDialog.Close(DialogResult.Ok(result));
    }

    private void RemoveCover()
    {
        var result = new CardCoverResult(null, CardCoverMode.None);
        MudDialog.Close(DialogResult.Ok(result));
    }

    private void Cancel() => MudDialog.Cancel();
}