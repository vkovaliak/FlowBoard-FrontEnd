using FlowBoard.Frontend.Domain.DTOs.Lists;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Http;

namespace FlowBoard.Frontend.Services.Implementations;

public class ListService : IListService
{
    private readonly IListApi _listApi;

    public ListService(IListApi listApi)
    {
        _listApi = listApi;
    }

    public async Task<bool> CreateAsync(CreateListDto list)
    {
        var response = await _listApi.CreateAsync(list);

        if (response.IsSuccessStatusCode && response.Content != Guid.Empty)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> UpdateAsync(Guid boardId, Guid listId, UpdateListDto list)
    {
        var response = await _listApi.UpdateAsync(boardId, listId, list);

        if (response.IsSuccessStatusCode && response.Content != false)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> DeleteAsync(Guid boardId, Guid listId)
    {
        var response = await _listApi.DeleteAsync(boardId, listId);

        if (response.IsSuccessStatusCode && response.Content != false)
        {
            return true;
        }

        return false;
    }
}