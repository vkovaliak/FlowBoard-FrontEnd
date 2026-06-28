using FlowBoard.Frontend.Domain.DTOs.Labels;
using FlowBoard.Frontend.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.Labels;

public partial class LabelManager
{
    [Inject] private ILabelService LabelService { get; set; } = default!;
    [Inject] private IDialogService DialogService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    [Parameter] public Guid BoardId { get; set; }
    [Parameter] public Guid CardId { get; set; }
    [Parameter] public List<LabelDto> AttachedLabels { get; set; } = [];

    private List<LabelDto> _boardLabels = [];
    private bool _isOpen;

    private bool IsAttached(LabelDto label)
        => AttachedLabels.Any(x => x.Id == label.Id);

    protected override async Task OnParametersSetAsync()
    {
        _boardLabels = await LabelService.GetByBoardIdAsync(BoardId);
    }

    private void TogglePopover() => _isOpen = !_isOpen;

    private async Task ReloadBoardLabelsAsync()
    {
        _boardLabels = await LabelService.GetByBoardIdAsync(BoardId);
        StateHasChanged();
    }

    private async Task ToggleAsync(LabelDto label)
    {
        if (IsAttached(label))
        {
            await DetachAsync(label);
        }
        else
        {
            await AttachAsync(label);
        }
    }

    private async Task AttachAsync(LabelDto label)
    {
        var result = await LabelService.AttachAsync(BoardId, CardId, label.Id);
        if (!result.Success)
        {
            Snackbar.Add(result.Error ?? "Failed", Severity.Error);
        }
    }

    private async Task DetachAsync(LabelDto label)
    {
        var result = await LabelService.DetachAsync(BoardId, CardId, label.Id);
        if (!result.Success)
        {
            Snackbar.Add(result.Error ?? "Failed", Severity.Error);
        }
    }

    private async Task OpenCreateAsync()
    {
        _isOpen = false;

        var parameters = new DialogParameters<CreateLabelDialog>
        {
            { x => x.IsEdit, false }
        };

        var dialog = await DialogService.ShowAsync<CreateLabelDialog>(
            null, parameters, SmallOptions());

        var dialogResult = await dialog.Result;

        if (dialogResult is null || dialogResult.Canceled) 
            return;

        if (dialogResult.Data is not (string name, string color)) 
            return;

        var dto = new CreateLabelDto(name, color);
        var result = await LabelService.CreateAsync(BoardId, dto);

        if (!result.Success)
        {
            Snackbar.Add(result.Error ?? "Failed", Severity.Error);
            return;
        }

        await LabelService.AttachAsync(BoardId, CardId, result.Value ?? Guid.Empty);
        await ReloadBoardLabelsAsync();
    }

    private async Task OpenManageAsync()
    {
        _isOpen = false;

        var parameters = new DialogParameters<ManageLabelsDialog>
        {
            { x => x.BoardId, BoardId }
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            CloseButton = false
        };

        var dialog = await DialogService.ShowAsync<ManageLabelsDialog>(
            null, parameters, options);

        await dialog.Result;
        
        await ReloadBoardLabelsAsync();
    }

    private static DialogOptions SmallOptions() => new()
    {
        MaxWidth = MaxWidth.ExtraSmall,
        FullWidth = true,
        CloseButton = false
    };
}