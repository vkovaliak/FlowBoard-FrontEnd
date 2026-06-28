using FlowBoard.Frontend.Domain.DTOs.Labels;
using FlowBoard.Frontend.Domain.Models.Common;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Helpers;
using FlowBoard.Frontend.Services.Http;

namespace FlowBoard.Frontend.Services.Implementations;

public class LabelService : ILabelService
{
    private readonly ILabelApi _labelApi;

    public LabelService(ILabelApi labelApi)
    {
        _labelApi = labelApi;
    }

    public async Task<OperationResult<Guid?>> CreateAsync(
        Guid boardId, CreateLabelDto dto)
    {
        var response = await _labelApi.CreateAsync(boardId, dto);

        if (response.IsSuccessStatusCode 
            && response.Content != Guid.Empty)
        {
            return OperationResult<Guid?>.Ok(response.Content);
        }

        return OperationResult<Guid?>.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> UpdateAsync(
        Guid boardId, Guid labelId, UpdateLabelDto dto)
    {
        var response = await _labelApi.UpdateAsync(
            boardId, labelId, dto);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> DeleteAsync(Guid boardId, Guid labelId)
    {
        var response = await _labelApi.DeleteAsync(boardId, labelId);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> AttachAsync(
        Guid boardId, Guid cardId, Guid labelId)
    {
        var response = await _labelApi.AttachAsync(
            boardId, cardId, labelId);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> DetachAsync(
        Guid boardId, Guid cardId, Guid labelId)
    {
        var response = await _labelApi.DetachAsync(
            boardId, cardId, labelId);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<List<LabelDto>> GetByBoardIdAsync(Guid boardId)
    {
        var response = await _labelApi.GetByBoardIdAsync(boardId);

        return response.IsSuccessStatusCode && response.Content is not null
        ? response.Content
        : [];
    }
}