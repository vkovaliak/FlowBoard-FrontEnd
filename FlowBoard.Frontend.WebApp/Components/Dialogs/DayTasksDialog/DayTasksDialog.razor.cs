using FlowBoard.Frontend.Domain.DTOs.Cards;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.DayTasksDialog;

public partial class DayTasksDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public DateTime Date { get; set; }
    [Parameter] public List<MyCardDto> Tasks { get; set; } = [];

    private void OnTaskClicked(MyCardDto task)
    {
        MudDialog.Close(DialogResult.Ok(task));
    }

    private void Cancel() => MudDialog.Cancel();
}