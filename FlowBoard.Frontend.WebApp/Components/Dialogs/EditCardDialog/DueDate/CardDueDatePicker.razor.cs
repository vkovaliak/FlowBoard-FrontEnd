using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.DueDate;

public partial class CardDueDatePicker
{
    [Parameter] public DateTime? DueDate { get; set; }
    [Parameter] public EventCallback<DateTime?> DueDateChanged { get; set; }
    [Parameter] public bool IsCompleted { get; set; }

    private DateTime? _date
    {
        get => DueDate;
        set => _ = SetDateAsync(value);
    }

    private async Task SetDateAsync(DateTime? value)
    {
        DueDate = value;
        await DueDateChanged.InvokeAsync(value);
    }

    private Color GetStatusColor()
    {
        if (IsCompleted)
        {
            return Color.Success;
        }

        if (DueDate < DateTime.Today)
        {
            return Color.Error;
        }

        if (DueDate == DateTime.Today)
        {
            return Color.Warning;
        }

        return Color.Default;
    }

    private string GetStatusText()
    {
        if (IsCompleted)
        {
            return "Completed";
        }

        if (DueDate < DateTime.Today)
        {
            return "Overdue";
        }

        if (DueDate == DateTime.Today)
        {
            return "Due today";
        }

        return "Upcoming";
    }
}