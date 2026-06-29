
using FlowBoard.Frontend.Domain.DTOs.Comments;
using FlowBoard.Frontend.Domain.Models.Common;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface ICommentService
{
    Task<OperationResult<Guid>> CreateAsync(Guid boardId, Guid cardId, CreateCommentDto dto);
    Task<IEnumerable<CommentDto>> GetCommentsAsync(Guid boardId, Guid cardId);
    Task<OperationResult> UpdateAsync(
        Guid boardId, Guid cardId, Guid commentId, UpdateCommentDto dto);
    Task<OperationResult> DeleteAsync(Guid boardId, Guid cardId, Guid commentId);
}