using FlowBoard.Frontend.Domain.DTOs.Attachments;
using FlowBoard.Frontend.Domain.Models.Common;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface IAttachmentService
{
    Task<OperationResult<AttachmentResponseDto?>> UploadCardAttachmentAsync(
        Guid boardId, Guid cardId, Stream fileStream, string fileName, string contentType);

    Task<OperationResult<AttachmentResponseDto?>> UploadCommentAttachmentAsync(
        Guid boardId, Guid cardId, Guid commentId, Stream fileStream, string fileName, string contentType);

    Task<OperationResult> DeleteCardAttachmentAsync(Guid boardId, Guid cardId, Guid attachmentId);
    
    Task<OperationResult> DeleteCommentAttachmentAsync(Guid boardId, Guid cardId, Guid attachmentId);
}