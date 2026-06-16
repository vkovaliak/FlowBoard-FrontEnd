using FlowBoard.Frontend.Domain.DTOs.Lists;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface IListService
{
    Task<bool> CreateAsync(Guid boardId, CreateListDto dto);
    Task<bool> UpdateAsync(Guid boardId, Guid listId, UpdateListDto dto);
    Task<bool> DeleteAsync(Guid boardId, Guid listId);
    Task<bool> MoveAsync(Guid boardId, Guid listId, MoveListDto dto);
}