using FlowBoard.Frontend.Domain.DTOs.Lists;
using FlowBoard.Frontend.Domain.Models.Common;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface IListService
{
    Task<OperationResult> CreateAsync(Guid boardId, CreateListDto dto);
    Task<OperationResult> UpdateAsync(Guid boardId, Guid listId, UpdateListDto dto);
    Task<OperationResult> DeleteAsync(Guid boardId, Guid listId);
    Task<OperationResult> MoveAsync(Guid boardId, Guid listId, MoveListDto dto);
}