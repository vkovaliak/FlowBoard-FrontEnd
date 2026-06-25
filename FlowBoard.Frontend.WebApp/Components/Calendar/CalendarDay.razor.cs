using FlowBoard.Frontend.Domain.DTOs.Cards;
using FlowBoard.Frontend.WebApp.Components.Dialogs.DayTasksDialog;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.Calendar;

public partial class CalendarDay
{
    [Inject] private IDialogService DialogService { get; set; } = default!;

    [Parameter] public DateTime Date { get; set; }
    [Parameter] public bool IsCurrentMonth { get; set; }
    [Parameter] public bool IsToday { get; set; }
    [Parameter] public List<MyCardDto> Tasks { get; set; } = [];
    [Parameter] public EventCallback<MyCardDto> OnTaskClick { get; set; }

    private async Task OpenDayDialogAsync()
    {
        var parameters = new DialogParameters<DayTasksDialog>
        {
            { x => x.Date, Date },
            { x => x.Tasks, Tasks }
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.ExtraSmall,
            CloseOnEscapeKey = true
        };

        var dialog = await DialogService.ShowAsync<DayTasksDialog>(
            null, parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled && result.Data is MyCardDto task)
        {
            await OnTaskClick.InvokeAsync(task);
        }
    }
}