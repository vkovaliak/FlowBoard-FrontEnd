using Refit;
using FlowBoard.Frontend.Domain.DTOs.Attachments;

namespace FlowBoard.Frontend.Services.Http;

public interface IAttachmentApi
{
    [Multipart]
    [Post("/api/attachment/card/{cardId}/upload")]
    Task<ApiResponse<AttachmentResponseDto>> UploadTaskAttachmentAsync(
        Guid cardId, StreamPart file);

    [Multipart]
    [Post("/api/attachment/comment/{commentId}/upload")]
    Task<ApiResponse<AttachmentResponseDto>> UploadCommentAttachmentAsync(
        Guid commentId, StreamPart file);
    
    [Delete("/api/attachment/card/{attachmentId}")]
    Task<ApiResponse<bool>> DeleteCardAttachmentAsync(Guid attachmentId);

    [Delete("/api/attachment/comment/{attachmentId}")]
    Task<ApiResponse<bool>> DeleteCommentAttachmentAsync(Guid attachmentId);
}
