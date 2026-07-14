using FlowBoard.Frontend.Domain.DTOs.Cards;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.BoardCalendarDayDialog;

public partial class BoardCalendarDayDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public DateTime Date { get; set; }
    [Parameter] public List<CardDto> Cards { get; set; } = [];

    private void OnCardClicked(CardDto card)
    {
        MudDialog.Close(DialogResult.Ok(card));
    }

    private void Cancel() => MudDialog.Cancel();
}