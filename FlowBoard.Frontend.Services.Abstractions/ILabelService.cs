using FlowBoard.Frontend.Domain.DTOs.Labels;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface ILabelService
{
    Task<Guid?> CreateAsync(Guid boardId, CreateLabelDto dto);
    Task<bool> UpdateAsync(Guid boardId, Guid labelId, UpdateLabelDto dto);
    Task<bool> DeleteAsync(Guid boardId, Guid labelId);
    Task<bool> AttachAsync(Guid boardId, Guid cardId, Guid labelId);
    Task<bool> DetachAsync(Guid boardId, Guid cardId, Guid labelId);
    Task<List<LabelDto>> GetByBoardIdAsync(Guid boardId);
}