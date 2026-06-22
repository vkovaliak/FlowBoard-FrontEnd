using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using FlowBoard.Frontend.Domain.DTOs.Attachments;
using FlowBoard.Frontend.Services.Abstractions;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.Shared;

public partial class AttachmentsSection
{
    [Inject] public IAttachmentService AttachmentService { get; set; } = default!;

    [Parameter] public Guid BoardId { get; set; }
    [Parameter] public Guid CardId { get; set; }
    [Parameter] public IReadOnlyList<AttachmentResponseDto> Attachments { get; set; } = [];
    [Parameter] public EventCallback<Guid> OnDelete { get; set; }

    private async Task HandleFilesSelected(InputFileChangeEventArgs e)
    {
        foreach (var file in e.GetMultipleFiles())
        {
            await UploadAsync(file);
        }
    }

    private async Task UploadAsync(IBrowserFile file)
    {
        await using var stream = file.OpenReadStream(5 * 1024 * 1024);
        await AttachmentService.UploadCardAttachmentAsync(
            BoardId, CardId, stream, file.Name, file.ContentType);
    }
}