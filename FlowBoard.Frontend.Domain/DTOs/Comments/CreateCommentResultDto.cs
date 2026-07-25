namespace FlowBoard.Frontend.Domain.DTOs.Comments;

public record CreateCommentResultDto(
    Guid CommentId,
    Guid? NotifyUserId
);