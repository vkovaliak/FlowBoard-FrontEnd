using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.Shared;

public partial class AttachmentUploader
{
    private readonly List<IBrowserFile> _pendingFiles = new();
    private string? _uploadingFile;

    private readonly string _inputId = $"file-input-{Guid.NewGuid():N}";

    public bool HasPendingFiles => _pendingFiles.Any();
    public IReadOnlyList<IBrowserFile> PendingFiles => _pendingFiles;

    private void OnFilesSelected(InputFileChangeEventArgs e)
    {
        _pendingFiles.AddRange(e.GetMultipleFiles());
    }

    private void RemoveFile(IBrowserFile file)
    {
        _pendingFiles.Remove(file);
    }

    public void Clear()
    {
        _pendingFiles.Clear();
        _uploadingFile = null;
    }

    public async Task UploadAllAsync(Func<IBrowserFile, Task> uploadAction)
    {
        foreach (var file in _pendingFiles.ToList())
        {
            _uploadingFile = file.Name;
            StateHasChanged();

            await uploadAction(file);
        }

        Clear();
        StateHasChanged();
    }

    private static string GetFileIcon(string contentType) =>
        contentType.StartsWith("image/")
            ? Icons.Material.Filled.Image
            : Icons.Material.Filled.InsertDriveFile;

    private static string FormatSize(long bytes) => bytes switch
    {
        < 1024 => $"{bytes} B",
        < 1024 * 1024 => $"{bytes / 1024.0:F1} KB",
        _ => $"{bytes / (1024.0 * 1024):F1} MB"
    };
}