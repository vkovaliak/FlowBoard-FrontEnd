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
        Guid boardId, Guid cardId, Stream fileStream, string fileName, string contentType)
    {
        var streamPart = new StreamPart(fileStream, fileName, contentType);

        var response = await _attachmentApi.UploadTaskAttachmentAsync(
            boardId, cardId, streamPart);

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            return response.Content;
        }

        return null;
    }

    public async Task<AttachmentResponseDto?> UploadCommentAttachmentAsync(
        Guid boardId, Guid cardId, Guid commentId, Stream fileStream, string fileName, string contentType)
    {
        var streamPart = new StreamPart(fileStream, fileName, contentType);

        var response = await _attachmentApi.UploadCommentAttachmentAsync(
            boardId, cardId, commentId, streamPart);

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            return response.Content;
        }

        return null;
    }

    public async Task<bool> DeleteCardAttachmentAsync(Guid boardId, Guid cardId, Guid attachmentId)
    {
        var response = await _attachmentApi.DeleteCardAttachmentAsync(boardId, cardId, attachmentId);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCommentAttachmentAsync(Guid boardId, Guid cardId, Guid attachmentId)
    {
        var response = await _attachmentApi.DeleteCommentAttachmentAsync(boardId, cardId, attachmentId);
        return response.IsSuccessStatusCode;
    }
}