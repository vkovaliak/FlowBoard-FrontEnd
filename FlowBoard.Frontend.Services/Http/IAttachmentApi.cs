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
}
