using FlowBoard.Frontend.Domain.DTOs.Labels;
using Refit;

namespace FlowBoard.Frontend.Services.Http;

public interface ILabelApi
{
    [Post("/api/boards/{boardId}/labels")]
    Task<ApiResponse<Guid>> CreateAsync(Guid boardId, [Body] CreateLabelDto dto);

    [Put("/api/boards/{boardId}/labels/{labelId}")]
    Task<ApiResponse<bool>> UpdateAsync(Guid boardId, Guid labelId, [Body] UpdateLabelDto dto);

    [Delete("/api/boards/{boardId}/labels/{labelId}")]
    Task<ApiResponse<bool>> DeleteAsync(Guid boardId, Guid labelId);

    [Post("/api/boards/{boardId}/cards/{cardId}/labels/{labelId}")]
    Task<ApiResponse<bool>> AttachAsync(Guid boardId, Guid cardId, Guid labelId);

    [Delete("/api/boards/{boardId}/cards/{cardId}/labels/{labelId}")]
    Task<ApiResponse<bool>> DetachAsync(Guid boardId, Guid cardId, Guid labelId);

    [Get("/api/boards/{boardId}/labels")]
    Task<ApiResponse<List<LabelDto>>> GetByBoardIdAsync(Guid boardId);
}