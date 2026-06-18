using FlowBoard.Frontend.Domain.DTOs.Checklists;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Http;

namespace FlowBoard.Frontend.Services.Implementations;

public class ChecklistService : IChecklistService
{
    private readonly IChecklistApi _checklistApi;

    public ChecklistService(IChecklistApi checklistApi)
    {
        _checklistApi = checklistApi;
    }

    public async Task<Guid?> AddAsync(
        Guid boardId, Guid cardId, AddChecklistItemDto dto)
    {
        var response = await _checklistApi.AddAsync(
            boardId, cardId, dto);

        if (response.IsSuccessStatusCode && response.Content != Guid.Empty)
        {
            return response.Content;
        }

        return null;
    }

    public async Task<bool> ToggleAsync(Guid boardId, Guid cardId, Guid itemId)
    {
        var response = await _checklistApi.ToggleAsync(
            boardId, cardId, itemId);

        return response.IsSuccessStatusCode
            && response.Content != false;
    }

    public async Task<bool> UpdateAsync(
        Guid boardId, Guid cardId, Guid itemId, UpdateChecklistItemDto dto)
    {
        var response = await _checklistApi.UpdateAsync(
            boardId, cardId, itemId, dto);

        return response.IsSuccessStatusCode
            && response.Content != false;
    }

    public async Task<bool> DeleteAsync(
        Guid boardId, Guid cardId, Guid itemId)
    {
        var response = await _checklistApi.DeleteAsync(
            boardId, cardId, itemId);

        return response.IsSuccessStatusCode
            && response.Content != false;
    }
}