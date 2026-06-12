namespace FlowBoard.Frontend.Services.Abstractions;

public interface IAttachmentService
{
    Task<string?> UploadAttachmentAsync(
        Stream fileStream, string fileName, string contentType);
}