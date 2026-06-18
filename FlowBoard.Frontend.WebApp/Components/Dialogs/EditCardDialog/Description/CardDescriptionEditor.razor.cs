using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using FlowBoard.Frontend.Services.Abstractions;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.Description;

public partial class CardDescriptionEditor
{
    [Inject] public IAttachmentService AttachmentService { get; set; } = default!;

    [Parameter] public Guid BoardId { get; set; }
    [Parameter] public Guid CardId { get; set; }
    [Parameter] public string? Description { get; set; }
    [Parameter] public EventCallback<string?> DescriptionChanged { get; set; }

    private bool _isEditing;

    private string? _description
    {
        get => Description;
        set => _ = SetDescriptionAsync(value);
    }

    private async Task SetDescriptionAsync(string? value)
    {
        Description = value;
        await DescriptionChanged.InvokeAsync(value);
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