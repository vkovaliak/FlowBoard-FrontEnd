using Microsoft.AspNetCore.Components;
using MudBlazor;
using FlowBoard.Frontend.Domain.DTOs.Cards;
using FlowBoard.Frontend.Services.Abstractions;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.StartDate;

public partial class CardStartDatePicker
{
    [Inject] private ICardService CardService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    [Parameter] public Guid BoardId { get; set; }
    [Parameter] public Guid CardId { get; set; }
    [Parameter] public DateTime? StartTime { get; set; }
    [Parameter] public bool CanEdit { get; set; } = true;

    private DateTime? _date;
    private bool _isEnabled;

    protected override void OnParametersSet()
    {
        _date = StartTime;
        _isEnabled = StartTime.HasValue;
    }

    private async Task OnToggleEnabledAsync(bool enabled)
    {
        _isEnabled = enabled;

        if (!enabled)
        {
            _date = null;
            await SaveAsync(null);
        }
        else if (_date is null)
        {
            _date = DateTime.Today;
            await SaveAsync(_date);
        }
    }

    private async Task OnDateChangedAsync(DateTime? value)
    {
        _date = value;
        await SaveAsync(value);
    }

    private async Task SaveAsync(DateTime? value)
    {
        var dto = new SetCardStartTimeDto(value);
        var result = await CardService.SetStartTimeAsync(BoardId, CardId, dto);

        if (!result.Success)
        {
            Snackbar.Add(result.Error ?? "Failed", Severity.Error);

            _date = StartTime;
            _isEnabled = StartTime.HasValue;
        }
    }
}