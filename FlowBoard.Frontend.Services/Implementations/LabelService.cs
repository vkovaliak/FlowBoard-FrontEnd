using FlowBoard.Frontend.Domain.DTOs.Labels;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Http;

namespace FlowBoard.Frontend.Services.Implementations;

public class LabelService : ILabelService
{
    private readonly ILabelApi _labelApi;

    public LabelService(ILabelApi labelApi)
    {
        _labelApi = labelApi;
    }

    public async Task<Guid?> CreateAsync(
        Guid boardId, CreateLabelDto dto)
    {
        var response = await _labelApi.CreateAsync(boardId, dto);

        if (response.IsSuccessStatusCode 
            && response.Content != Guid.Empty)
        {
            return response.Content;
        }

        return null;
    }

    public async Task<bool> UpdateAsync(
        Guid boardId, Guid labelId, UpdateLabelDto dto)
    {
        var response = await _labelApi.UpdateAsync(
            boardId, labelId, dto);

        return response.IsSuccessStatusCode
            && response.Content != false;
    }

    public async Task<bool> DeleteAsync(Guid boardId, Guid labelId)
    {
        var response = await _labelApi.DeleteAsync(boardId, labelId);

        return response.IsSuccessStatusCode
            && response.Content != false;
    }

    public async Task<bool> AttachAsync(
        Guid boardId, Guid cardId, Guid labelId)
    {
        var response = await _labelApi.AttachAsync(
            boardId, cardId, labelId);

        return response.IsSuccessStatusCode
            && response.Content != false;
    }

    public async Task<bool> DetachAsync(
        Guid boardId, Guid cardId, Guid labelId)
    {
        var response = await _labelApi.DetachAsync(
            boardId, cardId, labelId);

        return response.IsSuccessStatusCode
            && response.Content != false;
    }

    public async Task<List<LabelDto>> GetByBoardIdAsync(Guid boardId)
    {
        var response = await _labelApi.GetByBoardIdAsync(boardId);

        return response.IsSuccessStatusCode && response.Content is not null
        ? response.Content
        : [];
    }
}