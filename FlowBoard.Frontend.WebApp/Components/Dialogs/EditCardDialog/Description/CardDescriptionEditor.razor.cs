using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using FlowBoard.Frontend.Services.Abstractions;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.Description;

public partial class CardDescriptionEditor
{
    [Inject] public IAttachmentService AttachmentService { get; set; } = default!;

    [Parameter] public string? Description { get; set; }
    [Parameter] public EventCallback<string?> DescriptionChanged { get; set; }

    private bool _isEditing;

    private async Task OnDescriptionChanged(string? value)
    {
        Description = value;
        await DescriptionChanged.InvokeAsync(value);
    }

    private async Task HandleFileSelected(InputFileChangeEventArgs e)
    {
        foreach (var file in e.GetMultipleFiles())
        {
            await UploadAttachmentAsync(file);
        }
    }

    private async Task UploadAttachmentAsync(IBrowserFile file)
    {
        await using var stream = file.OpenReadStream(50 * 1024 * 1024);

        var url = await AttachmentService.UploadAttachmentAsync(
            stream, file.Name, file.ContentType);

        var html = file.ContentType.StartsWith("image/")
            ? $"<p><img src=\"{url}\" style=\"max-width:100%; border-radius:8px;\" /></p>"
            : $"<p><a href=\"{url}\" target=\"_blank\">{file.Name}</a></p>";

        await OnDescriptionChanged((Description ?? string.Empty) + html);
    }
}