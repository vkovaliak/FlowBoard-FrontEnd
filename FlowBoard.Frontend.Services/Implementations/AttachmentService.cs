using FlowBoard.Frontend.Services.Abstractions;
using Refit;

namespace FlowBoard.Frontend.Services.Http;

public class AttachmentService : IAttachmentService
{
    private readonly IAttachmentApi _attachmentApi;

    public AttachmentService(IAttachmentApi attachmentApi)
    {
        _attachmentApi = attachmentApi;
    }

    public async Task<string?> UploadAttachmentAsync(
        Stream fileStream, string fileName, string contentType)
    {

        var streamPart = new StreamPart(fileStream, fileName, contentType);

        var response = await _attachmentApi.UploadAsync(streamPart);

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            return response.Content;
        }

        return null;

    }
}