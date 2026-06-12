
using FlowBoard.Frontend.Domain.DTOs.Comments;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface ICommentService
{
    Task<bool> CreateAsync(Guid cardId, CreateCommentDto dto);
    Task<IEnumerable<CommentDto>> GetCommentsAsync(Guid cardId);
    Task<bool> UpdateAsync(Guid cardId, Guid commentId, UpdateCommentDto dto);
    Task<bool> DeleteAsync(Guid cardId, Guid commentId);
}