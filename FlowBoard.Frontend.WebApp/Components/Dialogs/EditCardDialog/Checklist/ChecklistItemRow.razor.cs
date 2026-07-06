using FlowBoard.Frontend.Domain.DTOs.Checklists;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.Checklist;

public partial class ChecklistItemRow
{
    [Parameter] public ChecklistItemDto Item { get; set; } = default!;
    [Parameter] public bool CanEdit { get; set; } = true;
    
    [Parameter] public EventCallback OnToggle { get; set; }
    [Parameter] public EventCallback OnDelete { get; set; }

    private async Task OnToggleAsync(bool _)
    {
        await OnToggle.InvokeAsync();
    }

    private async Task OnDeleteAsync()
    {
        await OnDelete.InvokeAsync();
    }

    private string GetTextStyle()
    {
        return Item.IsCompleted
            ? "text-decoration: line-through; color: #6b778c;"
            : string.Empty;
    }
}