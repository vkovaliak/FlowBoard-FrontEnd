namespace FlowBoard.Frontend.Domain.DTOs.Comments;

public record CreateCommentDto(
    string Message,
    Guid? MentionedUserId
);