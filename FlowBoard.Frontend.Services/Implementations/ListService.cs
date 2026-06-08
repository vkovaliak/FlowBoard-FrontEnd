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
}