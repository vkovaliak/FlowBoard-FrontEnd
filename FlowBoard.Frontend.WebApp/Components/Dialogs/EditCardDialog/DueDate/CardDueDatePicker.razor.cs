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
    [Parameter] public bool CanEdit { get; set; } = true;

    private DateTime? _date; 

    protected override void OnParametersSet()
    {
        _date = DueDate;
    }

    private async Task OnDateChangedAsync(DateTime? value)
    {
        _date = value;

        var dto = new SetCardDueDateDto(value);
        var result = await CardService.SetDueDateAsync(BoardId, CardId, dto);

        if (!result.Success)
        {
            Snackbar.Add(result.Error ?? "Failed", Severity.Error);
            _date = DueDate;
        }
    }
}