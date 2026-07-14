using FlowBoard.Frontend.Domain.DTOs.Cards;
using FlowBoard.Frontend.WebApp.Components.Dialogs.BoardCalendarDayDialog;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.BoardDetails.Calendar;

public partial class BoardCalendarDay
{
    [Inject] private IDialogService DialogService { get; set; } = default!;

    [Parameter] public DateTime Date { get; set; }
    [Parameter] public bool IsCurrentMonth { get; set; }
    [Parameter] public bool IsToday { get; set; }
    [Parameter] public List<CardDto> Cards { get; set; } = [];
    [Parameter] public EventCallback<CardDto> OnCardClick { get; set; }

    private async Task OpenDayDialogAsync()
    {
        var parameters = new DialogParameters<BoardCalendarDayDialog>
        {
            { x => x.Date, Date },
            { x => x.Cards, Cards }
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.ExtraSmall,
            CloseOnEscapeKey = true
        };

        var dialog = await DialogService.ShowAsync<BoardCalendarDayDialog>(
            null, parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled && result.Data is CardDto card)
        {
            await OnCardClick.InvokeAsync(card);
        }
    }
}