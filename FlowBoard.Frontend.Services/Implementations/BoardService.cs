using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Http;

namespace FlowBoard.Frontend.Services.Implementations;

public class BoardService : IBoardService
{
    private readonly IBoardApi _boardApi;

    public BoardService(IBoardApi boardApi)
    {
        _boardApi = boardApi;
    }

    public async Task<IEnumerable<BoardDto>> GetMyBoardsAsync()
    {
        var response = await _boardApi.GetMyBoardsAsync();

        if (response.IsSuccessStatusCode && response.Content != null )
        {
            return response.Content;
        }

        return [];
    }

    public async Task<bool> CreateAsync(CreateBoardDto board)
    {
        var response = await _boardApi.CreateAsync(board);

        if (response.IsSuccessStatusCode && response.Content != Guid.Empty)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> UpdateAsync(Guid boardId, UpdateBoardDto board)
    {
        var response = await _boardApi.UpdateAsync(boardId, board);

        if (response.IsSuccessStatusCode && response.Content != false)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> DeleteAsync(Guid boardId)
    {
        var response = await _boardApi.DeleteAsync(boardId);

        if (response.IsSuccessStatusCode && response.Content != false)
        {
            return true;
        }

        return false;
    }
}