using FlowBoard.Frontend.Domain.DTOs.Search;
using FlowBoard.Frontend.Domain.Models.Common;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface ISearchService
{
    Task<OperationResult<SearchResultDto>> SearchAsync(string query);
}