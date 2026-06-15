using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using FlowBoard.Frontend.Domain.DTOs.Attachments;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.Shared;

public partial class AttachmentList
{
    [Inject] public IJSRuntime Js { get; set; } = default!;

    [Parameter] public IReadOnlyList<AttachmentResponseDto> Attachments { get; set; } = [];

    private static bool IsImage(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        return ext is ".jpg" or ".jpeg" or ".png" or ".gif" or ".webp" or ".bmp";
    }

    private async Task OpenInNewTab(string url)
        => await Js.InvokeVoidAsync("open", url, "_blank");
}