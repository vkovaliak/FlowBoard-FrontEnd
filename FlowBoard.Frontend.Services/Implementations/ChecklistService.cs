using FlowBoard.Frontend.Domain.DTOs.Checklists;
using FlowBoard.Frontend.Domain.Models.Common;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Helpers;
using FlowBoard.Frontend.Services.Http;

namespace FlowBoard.Frontend.Services.Implementations;

public class ChecklistService : IChecklistService
{
    private readonly IChecklistApi _checklistApi;

    public ChecklistService(IChecklistApi checklistApi)
    {
        _checklistApi = checklistApi;
    }

    public async Task<OperationResult<Guid?>> AddAsync(
        Guid boardId, Guid cardId, AddChecklistItemDto dto)
    {
        var response = await _checklistApi.AddAsync(
            boardId, cardId, dto);

        if (response.IsSuccessStatusCode && response.Content != Guid.Empty)
        {
            return OperationResult<Guid?>.Ok(response.Content);
        }

        return OperationResult<Guid?>.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> ToggleAsync(
        Guid boardId, Guid cardId, Guid itemId)
    {
        var response = await _checklistApi.ToggleAsync(
            boardId, cardId, itemId);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> UpdateAsync(
        Guid boardId, Guid cardId, Guid itemId, UpdateChecklistItemDto dto)
    {
        var response = await _checklistApi.UpdateAsync(
            boardId, cardId, itemId, dto);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> DeleteAsync(
        Guid boardId, Guid cardId, Guid itemId)
    {
        var response = await _checklistApi.DeleteAsync(
            boardId, cardId, itemId);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }
}