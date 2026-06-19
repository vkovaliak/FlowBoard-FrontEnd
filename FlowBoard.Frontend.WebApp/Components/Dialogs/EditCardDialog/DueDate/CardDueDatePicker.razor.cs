using Microsoft.AspNetCore.Components;
using MudBlazor;
using FlowBoard.Frontend.Domain.DTOs.Cards;
using FlowBoard.Frontend.Services.Abstractions;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.DueDate;

public partial class CardDueDatePicker
{
    [Inject] private ICardService CardService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    [Parameter] public Guid BoardId { get; set; }
    [Parameter] public Guid CardId { get; set; }
    [Parameter] public DateTime? DueDate { get; set; }
    [Parameter] public bool IsCompleted { get; set; }

    private DateTime? _date
    {
        get => DueDate;
        set => _ = SetDateAsync(value);
    }

    private async Task SetDateAsync(DateTime? value)
    {
        var dto = new SetCardDueDateDto(value);
        var success = await CardService.SetDueDateAsync(
            BoardId, CardId, dto);

        if (!success)
        {
            Snackbar.Add("Failed to update due date", Severity.Error);
        }
    }

    private Color GetStatusColor()
    {
        if (IsCompleted) return Color.Success;
        if (DueDate < DateTime.Today) return Color.Error;
        if (DueDate == DateTime.Today) return Color.Warning;
        return Color.Default;
    }

    private string GetStatusText()
    {
        if (IsCompleted) return "Completed";
        if (DueDate < DateTime.Today) return "Overdue";
        if (DueDate == DateTime.Today) return "Due today";
        return "Upcoming";
    }
}