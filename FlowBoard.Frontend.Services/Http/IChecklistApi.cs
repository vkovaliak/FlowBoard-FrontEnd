using FlowBoard.Frontend.Domain.DTOs.Checklists;
using Refit;

namespace FlowBoard.Frontend.Services.Http;

public interface IChecklistApi
{
    [Post("/api/boards/{boardId}/cards/{cardId}/checklist")]
    Task<ApiResponse<Guid>> AddAsync(
        Guid boardId, Guid cardId, [Body] AddChecklistItemDto dto);

    [Put("/api/boards/{boardId}/cards/{cardId}/checklist/{itemId}/toggle")]
    Task<ApiResponse<bool>> ToggleAsync(Guid boardId, Guid cardId, Guid itemId);

    [Put("/api/boards/{boardId}/cards/{cardId}/checklist/{itemId}")]
    Task<ApiResponse<bool>> UpdateAsync(
        Guid boardId, Guid cardId, Guid itemId, [Body] UpdateChecklistItemDto dto);

    [Delete("/api/boards/{boardId}/cards/{cardId}/checklist/{itemId}")]
    Task<ApiResponse<bool>> DeleteAsync(Guid boardId, Guid cardId, Guid itemId);
}