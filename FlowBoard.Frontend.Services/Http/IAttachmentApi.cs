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
    [Post("/api/attachment/card/{cardId}/comment/{commentId}/upload")]
    Task<ApiResponse<AttachmentResponseDto>> UploadCommentAttachmentAsync(
       Guid cardId, Guid commentId, StreamPart file);
    
    [Delete("/api/attachment/card/{cardId}/attachment/{attachmentId}")]
    Task<ApiResponse<bool>> DeleteCardAttachmentAsync(Guid cardId, Guid attachmentId);

    [Delete("/api/attachment/card/{cardId}/comment/{attachmentId}")]
    Task<ApiResponse<bool>> DeleteCommentAttachmentAsync(Guid cardId, Guid attachmentId);
}
