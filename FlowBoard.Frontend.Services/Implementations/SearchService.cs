using FlowBoard.Frontend.Domain.DTOs.Search;
using FlowBoard.Frontend.Services.Abstractions;
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

    public async Task<SearchResultDto> SearchAsync(string query)
    {
        var trimmed = query?.Trim() ?? string.Empty;

        if (trimmed.Length < MinQueryLength)
        {
            return new SearchResultDto([], []);
        }

        var response = await _searchApi.SearchAsync(trimmed);

        if (!response.IsSuccessStatusCode || response.Content is null)
        {
            return new SearchResultDto([], []);  
        }

        return response.Content;
    }
}