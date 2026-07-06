using FlowBoard.Frontend.Domain.DTOs.Search;
using Refit;

namespace FlowBoard.Frontend.Services.Http;

public interface ISearchApi
{
    [Get("/api/search")]
    Task<ApiResponse<SearchResultDto>> SearchAsync(
        [Query] string query);

    [Get("/api/search/users")]
    Task<ApiResponse<List<UserSearchDto>>> SearchUsersAsync(
        [Query] string query);
}