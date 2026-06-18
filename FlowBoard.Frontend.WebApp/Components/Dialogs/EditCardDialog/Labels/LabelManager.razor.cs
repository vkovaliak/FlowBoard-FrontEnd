using FlowBoard.Frontend.Domain.Constants;
using FlowBoard.Frontend.Domain.DTOs.Labels;
using FlowBoard.Frontend.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.Labels;

public partial class LabelManager
{
    [Inject] private ILabelService LabelService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    [Parameter] public Guid BoardId { get; set; }
    [Parameter] public Guid CardId { get; set; }

    [Parameter] public List<LabelDto> BoardLabels { get; set; } = [];

    [Parameter] public List<LabelDto> AttachedLabels { get; set; } = [];

    [Parameter] public EventCallback OnChanged { get; set; }
    
    private List<LabelDto> _attached = [];
    private List<LabelDto> _boardLabels = [];
    private string _newName = string.Empty;
    private string _newColor = LabelColors.Default;
    private bool _isOpen;

    private bool IsAttached(LabelDto label)
        => _attached.Any(x => x.Id == label.Id);
    
    protected override async Task OnInitializedAsync()
    {
        await LoadBoardLabelsAsync();
    }

    private async Task LoadBoardLabelsAsync()
    {
        _attached = AttachedLabels.ToList();
        _boardLabels = await LabelService.GetByBoardIdAsync(BoardId);
    }

    private void TogglePopover()
    {
        _isOpen = !_isOpen;
    }

    private void SelectColor(string color)
    {
        _newColor = color;
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
        var success = await LabelService.AttachAsync(BoardId, CardId, label.Id);

        if (!success)
        {
            Snackbar.Add("Failed to attach label", Severity.Error);
            return;
        }

        await OnChanged.InvokeAsync();
    }

    private async Task DetachAsync(LabelDto label)
    {
        var success = await LabelService.DetachAsync(BoardId, CardId, label.Id);

        if (!success)
        {
            Snackbar.Add("Failed to detach label", Severity.Error);
            return;
        }

        await OnChanged.InvokeAsync();
    }

    private async Task CreateAndAttachAsync()
    {
        if (string.IsNullOrWhiteSpace(_newName))
        {
            return;
        }

        var dto = new CreateLabelDto(_newName.Trim(), _newColor);
        var newLabelId = await LabelService.CreateAsync(BoardId, dto);

        if (newLabelId is null)
        {
            Snackbar.Add("Failed to create label", Severity.Error);
            return;
        }

        await LabelService.AttachAsync(BoardId, CardId, newLabelId.Value);

        _newName = string.Empty;
        _newColor = LabelColors.Default;
        _isOpen = false;

        await LoadBoardLabelsAsync();
        await OnChanged.InvokeAsync();
    }

    private string GetSwatchStyle(string color)
    {
        var border = _newColor == color
            ? "3px solid #172b4d"
            : "1px solid #dfe1e6";

        return $"width: 28px; height: 28px; border-radius: 6px; " +
               $"background-color: {color}; cursor: pointer; border: {border};";
    }
}