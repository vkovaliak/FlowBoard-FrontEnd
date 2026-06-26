using FlowBoard.Frontend.Domain.DTOs.Search;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface ISearchService
{
    Task<SearchResultDto> SearchAsync(string query);
}