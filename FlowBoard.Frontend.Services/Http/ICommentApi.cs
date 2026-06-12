using FlowBoard.Frontend.Domain.DTOs.Comments;
using Refit;

namespace FlowBoard.Frontend.Services.Http;

public interface ICommentApi
{
    [Get("/api/comment/card/{cardId}")]
    Task<ApiResponse<IEnumerable<CommentDto>>> GetByCardIdAsync(Guid cardId);

    [Post("/api/comment/card/{cardId}")]
    Task<ApiResponse<Guid>> CreateAsync(Guid cardId, [Body] CreateCommentDto dto);

    [Put("/api/comment/card/{cardId}/comment/{commentId}")]
    Task<ApiResponse<bool>> UpdateAsync(Guid cardId, Guid commentId, [Body] UpdateCommentDto dto);

    [Delete("/api/comment/card/{cardId}/comment/{commentId}")]
    Task<ApiResponse<bool>> DeleteAsync(Guid cardId, Guid commentId);
}