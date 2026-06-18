using FlowBoard.Frontend.Domain.DTOs.Attachments;

namespace FlowBoard.Frontend.Domain.DTOs.Comments;

public record CommentDto(
    Guid Id,
    Guid CardId,
    string Message,
    DateTime CreatedAt,
    Guid CreatedBy,
    string Email,
    string UserName,
    string? AvatarUrl,
    List<AttachmentResponseDto> Attachments
);