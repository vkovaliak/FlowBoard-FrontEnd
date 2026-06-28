using FlowBoard.Frontend.Domain.DTOs.Labels;
using FlowBoard.Frontend.Domain.Models.Common;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface ILabelService
{
    Task<OperationResult<Guid?>> CreateAsync(Guid boardId, CreateLabelDto dto);
    Task<OperationResult> UpdateAsync(Guid boardId, Guid labelId, UpdateLabelDto dto);
    Task<OperationResult> DeleteAsync(Guid boardId, Guid labelId);
    Task<OperationResult> AttachAsync(Guid boardId, Guid cardId, Guid labelId);
    Task<OperationResult> DetachAsync(Guid boardId, Guid cardId, Guid labelId);
    Task<List<LabelDto>> GetByBoardIdAsync(Guid boardId);
}