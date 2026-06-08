using FlowBoard.Frontend.Domain.DTOs.Lists;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface IListService
{
    Task<bool> CreateAsync(CreateListDto dto);
}