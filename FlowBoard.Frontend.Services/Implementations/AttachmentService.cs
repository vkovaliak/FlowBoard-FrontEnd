using FlowBoard.Frontend.Domain.DTOs.Attachments;
using FlowBoard.Frontend.Domain.Models.Common;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Helpers;
using Refit;

namespace FlowBoard.Frontend.Services.Http;

public class AttachmentService : IAttachmentService
{
    private readonly IAttachmentApi _attachmentApi;

    public AttachmentService(IAttachmentApi attachmentApi)
    {
        _attachmentApi = attachmentApi;
    }

    public async Task<OperationResult<AttachmentResponseDto?>> UploadCardAttachmentAsync(
        Guid boardId, Guid cardId, Stream fileStream, string fileName, string contentType)
    {
        var streamPart = new StreamPart(fileStream, fileName, contentType);

        var response = await _attachmentApi.UploadTaskAttachmentAsync(
            boardId, cardId, streamPart);

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            return OperationResult<AttachmentResponseDto?>.Ok(response.Content);
        }

        return OperationResult<AttachmentResponseDto?>.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult<AttachmentResponseDto?>> UploadCommentAttachmentAsync(
        Guid boardId, Guid cardId, Guid commentId, Stream fileStream, string fileName, string contentType)
    {
        var streamPart = new StreamPart(fileStream, fileName, contentType);

        var response = await _attachmentApi.UploadCommentAttachmentAsync(
            boardId, cardId, commentId, streamPart);

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            return OperationResult<AttachmentResponseDto?>.Ok(response.Content);
        }

        return OperationResult<AttachmentResponseDto?>.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> DeleteCardAttachmentAsync(Guid boardId, Guid cardId, Guid attachmentId)
    {
        var response = await _attachmentApi.DeleteCardAttachmentAsync(boardId, cardId, attachmentId);
        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> DeleteCommentAttachmentAsync(Guid boardId, Guid cardId, Guid attachmentId)
    {
        var response = await _attachmentApi.DeleteCommentAttachmentAsync(boardId, cardId, attachmentId);
        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }
}