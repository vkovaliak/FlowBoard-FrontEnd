using FlowBoard.Frontend.Domain.DTOs.Comments;
using Refit;

namespace FlowBoard.Frontend.Services.Http;

public interface ICommentApi
{
    [Get("/api/boards/{boardId}/cards/{cardId}/comments")]
    Task<ApiResponse<IEnumerable<CommentDto>>> GetByCardIdAsync(Guid boardId, Guid cardId);

    [Post("/api/boards/{boardId}/cards/{cardId}/comments")]
    Task<ApiResponse<CreateCommentResultDto>> CreateAsync(Guid boardId, Guid cardId, [Body] CreateCommentDto dto);

    [Put("/api/boards/{boardId}/cards/{cardId}/comments/{commentId}")]
    Task<ApiResponse<bool>> UpdateAsync(Guid boardId, Guid cardId, Guid commentId, [Body] UpdateCommentDto dto);

    [Delete("/api/boards/{boardId}/cards/{cardId}/comments/{commentId}")]
    Task<ApiResponse<bool>> DeleteAsync(Guid boardId, Guid cardId, Guid commentId);
}