using FlowBoard.Frontend.Domain.DTOs.Lists;
using Refit;

namespace FlowBoard.Frontend.Services.Http;

public interface IListApi
{
    [Post("/api/boards/{boardId}/lists")]
    Task<ApiResponse<Guid>> CreateAsync(Guid boardId, [Body] CreateListDto dto);

    [Put("/api/boards/{boardId}/lists/{listId}")]
    Task<ApiResponse<bool>> UpdateAsync(Guid boardId, Guid listId, [Body] UpdateListDto dto);

    [Delete("/api/boards/{boardId}/lists/{listId}")]
    Task<ApiResponse<bool>> DeleteAsync(Guid boardId, Guid listId);

    [Put("/api/boards/{boardId}/lists/{listId}/move")]
    Task<ApiResponse<bool>> MoveAsync(Guid boardId, Guid listId, [Body] MoveListDto dto);
}