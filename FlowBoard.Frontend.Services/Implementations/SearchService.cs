using FlowBoard.Frontend.Domain.DTOs.Search;
using FlowBoard.Frontend.Domain.Models.Common;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Helpers;
using FlowBoard.Frontend.Services.Http;

namespace FlowBoard.Frontend.Services.Implementations;

public class SearchService : ISearchService
{
    private const int MinQueryLength = 3;

    private readonly ISearchApi _searchApi;

    public SearchService(ISearchApi searchApi)
    {
        _searchApi = searchApi;
    }

    public async Task<OperationResult<SearchResultDto>> SearchAsync(string query)
    {
        var trimmed = query?.Trim() ?? string.Empty;

        if (trimmed.Length < MinQueryLength)
        {
            return OperationResult<SearchResultDto>.Ok(new SearchResultDto([], []));
        }

        var response = await _searchApi.SearchAsync(trimmed);

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            return OperationResult<SearchResultDto>.Ok(response.Content);
        }

        return OperationResult<SearchResultDto>.Fail(response.GetErrorMessage());
    }
}