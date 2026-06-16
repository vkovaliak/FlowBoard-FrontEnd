
using FlowBoard.Frontend.Domain.DTOs.Comments;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface ICommentService
{
    Task<Guid> CreateAsync(Guid boardId, Guid cardId, CreateCommentDto dto);
    Task<IEnumerable<CommentDto>> GetCommentsAsync(Guid boardId, Guid cardId);
    Task<bool> UpdateAsync(Guid boardId, Guid cardId, Guid commentId, UpdateCommentDto dto);
    Task<bool> DeleteAsync(Guid boardId, Guid cardId, Guid commentId);
}