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

    private string _name = string.Empty;

    protected override void OnParametersSet()
    {
        if (_name != Name && !_isEditing)
        {
            _name = Name;
        }
    }

    private bool _isEditing;

    private async Task SaveNameAsync()
    {
        _isEditing = false;

        if (string.IsNullOrWhiteSpace(_name) || _name == Name)
        {
            _name = Name;
            return;
        }

        var dto = new RenameCardDto(_name);
        var success = await CardService.RenameAsync(
            BoardId, CardId, dto);

        if (!success)
        {
            Snackbar.Add("Failed to rename card", Severity.Error);
            _name = Name;
        }
    }
}