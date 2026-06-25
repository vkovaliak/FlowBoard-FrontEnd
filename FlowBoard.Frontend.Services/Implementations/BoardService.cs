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

        return response.IsSuccessStatusCode 
            && response.Content != null ? response.Content : [];
    }

    public async Task<BoardDetailsDto?> GetDetailsAsync(Guid boardId)
    {
        var response = await _boardApi.GetDetailsAsync(boardId);

        return response.IsSuccessStatusCode 
            ? response.Content : null;
    }

    public async Task<bool> CreateAsync(CreateBoardDto board)
    {
        var response = await _boardApi.CreateAsync(board);

        return response.IsSuccessStatusCode 
            && response.Content != Guid.Empty;
    }

    public async Task<bool> UpdateAsync(Guid boardId, UpdateBoardDto board)
    {
        var response = await _boardApi.UpdateAsync(boardId, board);

        return response.IsSuccessStatusCode 
            && response.Content != false;
    }

    public async Task<bool> DeleteAsync(Guid boardId)
    {
        var response = await _boardApi.DeleteAsync(boardId);

        return response.IsSuccessStatusCode 
            && response.Content != false;
    }

    public async Task<bool> InviteMemberAsync(Guid boardId, InviteMemberDto dto)
    {
        var response = await _boardApi.InviteMemberAsync(boardId, dto);

        return response.IsSuccessStatusCode 
            && response.Content != false;
    }

    public async Task<bool> RemoveMemberAsync(Guid boardId, Guid userId)
    {
        var response = await _boardApi.RemoveMemberAsync(boardId, userId);

        return response.IsSuccessStatusCode
            && response.Content != false;
    }

    public async Task<bool> LeaveBoardAsync(Guid boardId)
    {
        var response = await _boardApi.LeaveBoardAsync(boardId);

        return response.IsSuccessStatusCode
            && response.Content != false;
    }

    public async Task<bool> ToggleFavoriteAsync(Guid boardId)
    {
        var response = await _boardApi.ToggleFavoriteAsync(boardId);

        return response.IsSuccessStatusCode
            && response.Content != false;
    }
}