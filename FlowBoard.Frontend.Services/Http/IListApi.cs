using FlowBoard.Frontend.Domain.DTOs.Lists;
using Refit;

namespace FlowBoard.Frontend.Services.Http;

public interface IListApi
{
    [Post("/api/lists")]
    Task<ApiResponse<Guid>> CreateAsync([Body] CreateListDto dto);
}