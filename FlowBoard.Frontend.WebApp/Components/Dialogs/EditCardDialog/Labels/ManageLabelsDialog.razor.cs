using Microsoft.AspNetCore.Components;
using MudBlazor;
using FlowBoard.Frontend.Domain.DTOs.Labels;
using FlowBoard.Frontend.Services.Abstractions;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.Labels;

public partial class ManageLabelsDialog
{
    [Inject] private ILabelService LabelService { get; set; } = default!;
    [Inject] private IDialogService DialogService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    [CascadingParameter] public IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public Guid BoardId { get; set; }

    private List<LabelDto> _labels = [];

    protected override async Task OnInitializedAsync()
    {
        await ReloadAsync();
    }

    private async Task ReloadAsync()
    {
        _labels = await LabelService.GetByBoardIdAsync(BoardId);
    }

    private async Task CreateAsync()
    {
        var parameters = new DialogParameters<CreateLabelDialog>
        {
            { x => x.IsEdit, false }
        };

        var dialog = await DialogService.ShowAsync<CreateLabelDialog>(
            null, parameters, SmallOptions());

        var result = await dialog.Result;
        if (result is null || result.Canceled) 
            return;

        if (result.Data is not (string name, string color)) 
            return;

        var dto = new CreateLabelDto(name, color);
        var success = await LabelService.CreateAsync(BoardId, dto);

        if (success is null)
        {
            Snackbar.Add("Failed to create label", Severity.Error);
            return;
        }

        await ReloadAsync();
    }

    private async Task EditAsync(LabelDto label)
    {
        var parameters = new DialogParameters<CreateLabelDialog>
        {
            { x => x.IsEdit, true },
            { x => x.InitialName, label.Name },
            { x => x.InitialColor, label.Color }
        };

        var dialog = await DialogService.ShowAsync<CreateLabelDialog>(
            null, parameters, SmallOptions());

        var dialogResult = await dialog.Result;
        if (dialogResult is null || dialogResult.Canceled) 
            return;

        if (dialogResult.Data is not (string name, string color)) 
            return;

        var dto = new UpdateLabelDto(name, color);
        var result = await LabelService.UpdateAsync(BoardId, label.Id, dto);

        if (!result.Success)
        {
            Snackbar.Add(result.Error ?? "Failed", Severity.Error);
            return;
        }

        await ReloadAsync();
    }

    private async Task DeleteAsync(LabelDto label)
    {
        var result = await LabelService.DeleteAsync(BoardId, label.Id);

        if (!result.Success)
        {
            Snackbar.Add(result.Error ?? "Failed", Severity.Error);
            return;
        }

        await ReloadAsync();
    }

    private static DialogOptions SmallOptions() => new()
    {
        MaxWidth = MaxWidth.ExtraSmall,
        FullWidth = true,
        CloseButton = false
    };

    private void Close() => MudDialog.Close();
}