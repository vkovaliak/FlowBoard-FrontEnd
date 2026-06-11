using FlowBoard.Frontend.Domain.DTOs.Comments;
using Refit;

namespace FlowBoard.Frontend.Services.Http;

public interface ICommentApi
{
    [Get("/api/comment/card/{cardId}")]
    Task<ApiResponse<IEnumerable<CommentDto>>> GetByCardIdAsync(Guid cardId);

    [Post("/api/comment/card/{cardId}")]
    Task<ApiResponse<Guid>> CreateAsync(Guid cardId, [Body] CreateCommentDto dto);
}