using FlowBoard.Frontend.Domain.DTOs.Checklists;
using FlowBoard.Frontend.Domain.Models.Common;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface IChecklistService
{
    Task<OperationResult<Guid?>> AddAsync(Guid boardId, Guid cardId, AddChecklistItemDto dto);
    Task<OperationResult> ToggleAsync(Guid boardId, Guid cardId, Guid itemId);
    Task<OperationResult> UpdateAsync(Guid boardId, Guid cardId, Guid itemId, UpdateChecklistItemDto dto);
    Task<OperationResult> DeleteAsync(Guid boardId, Guid cardId, Guid itemId);
}