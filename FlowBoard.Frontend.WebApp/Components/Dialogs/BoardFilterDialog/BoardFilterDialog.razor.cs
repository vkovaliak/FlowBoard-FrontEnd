using FlowBoard.Frontend.Domain.DTOs.Labels;
using FlowBoard.Frontend.Domain.Models.Cards;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.BoardFilterDialog;

public partial class BoardFilterDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public CardFilterModel Filter { get; set; } = new();
    [Parameter] public List<LabelDto> Labels { get; set; } = [];

    private CardFilterModel _filter = new();

    protected override void OnInitialized()
    {
        _filter = Filter.Clone();
    }

    private void ToggleLabel(Guid labelId, bool selected)
    {
        if (selected)
        {
            _filter.LabelIds.Add(labelId);
        }
        else
        {
            _filter.LabelIds.Remove(labelId);
        }
    }

    private void ResetFilter()
    {
        _filter.Reset();
    }

    private void Apply()
        => MudDialog.Close(DialogResult.Ok(_filter));

    private void Cancel()
        => MudDialog.Cancel();
}