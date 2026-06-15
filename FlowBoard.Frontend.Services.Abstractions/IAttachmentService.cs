using FlowBoard.Frontend.Domain.DTOs.Attachments;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface IAttachmentService
{
    Task<AttachmentResponseDto?> UploadCardAttachmentAsync(
        Guid cardId, Stream fileStream, string fileName, string contentType);

    Task<AttachmentResponseDto?> UploadCommentAttachmentAsync(
        Guid commentId, Stream fileStream, string fileName, string contentType);

    Task<bool> DeleteCardAttachmentAsync(Guid attachmentId);
    
    Task<bool> DeleteCommentAttachmentAsync(Guid attachmentId);
}