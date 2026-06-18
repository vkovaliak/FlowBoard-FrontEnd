using FlowBoard.Frontend.Domain.DTOs.Checklists;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface IChecklistService
{
    Task<Guid?> AddAsync(Guid boardId, Guid cardId, AddChecklistItemDto dto);
    Task<bool> ToggleAsync(Guid boardId, Guid cardId, Guid itemId);
    Task<bool> UpdateAsync(Guid boardId, Guid cardId, Guid itemId, UpdateChecklistItemDto dto);
    Task<bool> DeleteAsync(Guid boardId, Guid cardId, Guid itemId);
}