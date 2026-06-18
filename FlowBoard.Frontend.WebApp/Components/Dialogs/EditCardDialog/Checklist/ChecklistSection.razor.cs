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
    [Parameter] public EventCallback OnChanged { get; set; }

    private List<ChecklistItemDto> _items = [];
    private bool _isAdding;
    private string _newText = string.Empty;

    protected override void OnInitialized()
    {
        _items = Items.ToList();
    }

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

        var dto = new AddChecklistItemDto(_newText.Trim());
        var newId = await ChecklistService.AddAsync(
            BoardId, CardId, dto);

        if (newId is null)
        {
            Snackbar.Add("Failed to add item", Severity.Error);
            return;
        }

        await OnChanged.InvokeAsync();
    }

    private async Task ToggleAsync(ChecklistItemDto item)
    {
        var success = await ChecklistService.ToggleAsync(BoardId, CardId, item.Id);

        if (!success)
        {
            Snackbar.Add("Failed to update item", Severity.Error);
            return;
        }

        await OnChanged.InvokeAsync();
    }

    private async Task DeleteAsync(ChecklistItemDto item)
    {
        var success = await ChecklistService.DeleteAsync(BoardId, CardId, item.Id);

        if (!success)
        {
            Snackbar.Add("Failed to delete item", Severity.Error);
            return;
        }

        await OnChanged.InvokeAsync();
    }
}