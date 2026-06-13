using Microsoft.AspNetCore.Components;
using MudBlazor;
using FlowBoard.Frontend.Domain.DTOs.Cards;
using FlowBoard.Frontend.Domain.Models.Cards;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog;

public partial class EditCardDialog : ComponentBase
{
    [CascadingParameter] public IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public Guid CardId { get; set; }
    [Parameter] public string CurrentName { get; set; } = string.Empty;
    [Parameter] public string? CurrentDescription { get; set; }

    private CreateCardModel _model = new();

    protected override void OnInitialized()
    {
        _model.Name = CurrentName;
        _model.Description = CurrentDescription;
    }

    private void Cancel() => MudDialog.Cancel();

    private void Save()
    {
        if (string.IsNullOrWhiteSpace(_model.Name))
        {
            return;
        }

        MudDialog.Close(DialogResult.Ok(
            new UpdateCardDto(_model.Name, _model.Description ?? string.Empty)));
    }
}