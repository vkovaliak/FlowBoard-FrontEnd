using FlowBoard.Frontend.Domain.DTOs.Lists;
using FlowBoard.Frontend.Domain.Models.Common;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Helpers;
using FlowBoard.Frontend.Services.Http;

namespace FlowBoard.Frontend.Services.Implementations;

public class ListService : IListService
{
    private readonly IListApi _listApi;

    public ListService(IListApi listApi)
    {
        _listApi = listApi;
    }

    public async Task<OperationResult> CreateAsync(Guid boardId, CreateListDto list)
    {
        var response = await _listApi.CreateAsync(boardId, list);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> UpdateAsync(Guid boardId, Guid listId, UpdateListDto list)
    {
        var response = await _listApi.UpdateAsync(boardId, listId, list);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> DeleteAsync(Guid boardId, Guid listId)
    {
        var response = await _listApi.DeleteAsync(boardId, listId);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> MoveAsync(Guid boardId, Guid listId, MoveListDto list)
    {
        var response = await _listApi.MoveAsync(boardId, listId, list);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }
}