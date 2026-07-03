using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using FlowBoard.Frontend.Domain.DTOs.Cards;
using FlowBoard.Frontend.Services.Abstractions;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.Description;

public partial class CardDescriptionEditor
{
    [Inject] public IAttachmentService AttachmentService { get; set; } = default!;
    [Inject] private ICardService CardService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    [Parameter] public Guid BoardId { get; set; }
    [Parameter] public Guid CardId { get; set; }
    [Parameter] public string? Description { get; set; }
    [Parameter] public bool CanEdit { get; set; } = true;

    private bool _isEditing;
    private string? _draft; 

    private void StartEditing()
    {
        _draft = Description;
        _isEditing = true;
    }

    private void CancelEditing()
    {
        _isEditing = false;
        _draft = null;
    }

    private async Task SaveAsync()
    {
        var dto = new UpdateCardDescriptionDto(_draft);
        var result = await CardService.UpdateDescriptionAsync(
            BoardId, CardId, dto);

        if (!result.Success)
        {
            Snackbar.Add(result.Error ?? "Failed", Severity.Error);
            return;
        }

        _isEditing = false;
        _draft = null;
    }

    private async Task HandleFilesSelected(InputFileChangeEventArgs e)
    {
        foreach (var file in e.GetMultipleFiles())
        {
            await UploadAsync(file);
        }
    }

    private async Task UploadAsync(IBrowserFile file)
    {
        await using var stream = file.OpenReadStream(50 * 1024 * 1024);
        await AttachmentService.UploadCardAttachmentAsync(
            BoardId, CardId, stream, file.Name, file.ContentType);
    }
}