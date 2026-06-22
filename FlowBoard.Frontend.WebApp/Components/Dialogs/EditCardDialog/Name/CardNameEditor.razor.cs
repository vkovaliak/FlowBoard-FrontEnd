using Microsoft.AspNetCore.Components;
using MudBlazor;
using FlowBoard.Frontend.Domain.DTOs.Cards;
using FlowBoard.Frontend.Services.Abstractions;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.Name;

public partial class CardNameEditor
{
    [Inject] private ICardService CardService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    [Parameter] public Guid BoardId { get; set; }
    [Parameter] public Guid CardId { get; set; }
    [Parameter] public string Name { get; set; } = string.Empty;

    private bool _isEditing;
    private string _draft = string.Empty;

    private void StartEditing()
    {
        _draft = Name;
        _isEditing = true;
    }

    private void CancelEditing()
    {
        _isEditing = false;
        _draft = string.Empty;
    }

    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(_draft) || _draft == Name)
        {
            CancelEditing();
            return;
        }

        var dto = new RenameCardDto(_draft);
        var success = await CardService.RenameAsync(
            BoardId, CardId, dto);

        if (!success)
        {
            Snackbar.Add("Failed to rename card", Severity.Error);
            return;
        }

        _isEditing = false;
        _draft = string.Empty;
    }
}