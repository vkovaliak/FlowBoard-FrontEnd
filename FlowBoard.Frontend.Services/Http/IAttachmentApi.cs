using Refit;
using FlowBoard.Frontend.Domain.DTOs.Attachments;

namespace FlowBoard.Frontend.Services.Http;

public interface IAttachmentApi
{
    [Multipart]
    [Post("/api/boards/{boardId}/card/{cardId}/upload")]
    Task<ApiResponse<AttachmentResponseDto>> UploadTaskAttachmentAsync(
        Guid boardId, Guid cardId, StreamPart file);

    [Multipart]
    [Post("/api/boards/{boardId}/card/{cardId}/comment/{commentId}/upload")]
    Task<ApiResponse<AttachmentResponseDto>> UploadCommentAttachmentAsync(
       Guid boardId, Guid cardId, Guid commentId, StreamPart file);
    
    [Delete("/api/boards/{boardId}/card/{cardId}/attachment/{attachmentId}")]
    Task<ApiResponse<bool>> DeleteCardAttachmentAsync(
        Guid boardId, Guid cardId, Guid attachmentId);

    [Delete("/api/boards/{boardId}/card/{cardId}/comment/{attachmentId}")]
    Task<ApiResponse<bool>> DeleteCommentAttachmentAsync(
        Guid boardId, Guid cardId, Guid attachmentId);
}
