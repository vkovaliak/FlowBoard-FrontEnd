using FlowBoard.Frontend.Domain.DTOs.Attachments;
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

    public async Task<AttachmentResponseDto?> UploadCardAttachmentAsync(
        Guid cardId, Stream fileStream, string fileName, string contentType)
    {
        var streamPart = new StreamPart(fileStream, fileName, contentType);

        var response = await _attachmentApi.UploadTaskAttachmentAsync(
            cardId, streamPart);

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            return response.Content;
        }

        return null;
    }

    public async Task<AttachmentResponseDto?> UploadCommentAttachmentAsync(
        Guid commentId, Stream fileStream, string fileName, string contentType)
    {
        var streamPart = new StreamPart(fileStream, fileName, contentType);

        var response = await _attachmentApi.UploadCommentAttachmentAsync(
            commentId, streamPart);

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            return response.Content;
        }

        return null;
    }

    public async Task<bool> DeleteCardAttachmentAsync(Guid attachmentId)
    {
        var response = await _attachmentApi.DeleteCardAttachmentAsync(attachmentId);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCommentAttachmentAsync(Guid attachmentId)
    {
        var response = await _attachmentApi.DeleteCommentAttachmentAsync(attachmentId);
        return response.IsSuccessStatusCode;
    }
}