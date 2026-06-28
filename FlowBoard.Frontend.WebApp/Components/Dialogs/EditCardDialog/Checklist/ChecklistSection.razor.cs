using FlowBoard.Frontend.Domain.DTOs.Checklists;
using FlowBoard.Frontend.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.Checklist;

public partial class ChecklistSection
{
    [Inject] private IChecklistService ChecklistService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    [Parameter] public Guid BoardId { get; set; }
    [Parameter] public Guid CardId { get; set; }
    [Parameter] public List<ChecklistItemDto> Items { get; set; } = [];

    private bool _isAdding;
    private string _newText = string.Empty;

    private void StartAdd()
    {
        _isAdding = true;
        _newText = string.Empty;
    }

    private void CancelAdd()
    {
        _isAdding = false;
        _newText = string.Empty;
    }

    private async Task OnKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter" && !string.IsNullOrWhiteSpace(_newText))
        {
            await AddAsync();
        }
    }

    private async Task AddAsync()
    {
        if (string.IsNullOrWhiteSpace(_newText))
        {
            return;
        }

        var dto = new AddChecklistItemDto(_newText);
        var result = await ChecklistService.AddAsync(BoardId, CardId, dto);

        if (!result.Success)
        {
            Snackbar.Add(result.Error ?? "Failed to add item", Severity.Error);
            return;
        }

        _isAdding = false;
        _newText = string.Empty;
    }

    private async Task ToggleAsync(ChecklistItemDto item)
    {
        var result = await ChecklistService.ToggleAsync(
            BoardId, CardId, item.Id);

        if (!result.Success)
        {
            Snackbar.Add(result.Error ?? "Failed", Severity.Error);
        }
    }

    private async Task DeleteAsync(ChecklistItemDto item)
    {
        var result = await ChecklistService.DeleteAsync(
            BoardId, CardId, item.Id);

        if (!result.Success)
        {
            Snackbar.Add(result.Error ?? "Failed", Severity.Error);
        }
    }
}