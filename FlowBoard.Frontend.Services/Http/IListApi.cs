using FlowBoard.Frontend.Domain.DTOs.Lists;
using Refit;

namespace FlowBoard.Frontend.Services.Http;

public interface IListApi
{
    [Post("/api/lists")]
    Task<ApiResponse<Guid>> CreateAsync([Body] CreateListDto dto);

    [Put("/api/lists/{boardId}/list/{listId}")]
    Task<ApiResponse<bool>> UpdateAsync(Guid boardId, Guid listId, [Body] UpdateListDto dto);

    [Delete("/api/lists/{boardId}/list/{listId}")]
    Task<ApiResponse<bool>> DeleteAsync(Guid boardId, Guid listId);

    [Put("/api/lists/{boardId}/list/{listId}/move")]
    Task<ApiResponse<bool>> MoveAsync(Guid boardId, Guid listId, [Body] MoveListDto dto);
}