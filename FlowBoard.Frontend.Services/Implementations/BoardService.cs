using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Domain.Models.Common;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Helpers;
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

    public async Task<IEnumerable<BoardDto>> GetArchivedBoardsAsync()
    {
        var response = await _boardApi.GetArchivedBoardsAsync();

        return response.IsSuccessStatusCode 
            && response.Content != null ? response.Content : [];
    }

    public async Task<BoardDetailsDto?> GetDetailsAsync(Guid boardId)
    {
        var response = await _boardApi.GetDetailsAsync(boardId);

        return response.IsSuccessStatusCode 
            ? response.Content : null;
    }

    public async Task<OperationResult<Guid>> CreateAsync(CreateBoardDto board)
    {
        var response = await _boardApi.CreateAsync(board);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult<Guid>.Ok(response.Content);
        }

        return OperationResult<Guid>.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult<Guid>> UpdateAsync(Guid boardId, UpdateBoardDto board)
    {
        var response = await _boardApi.UpdateAsync(boardId, board);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult<Guid>.Ok(response.Content);
        }

        return OperationResult<Guid>.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> DeleteAsync(Guid boardId)
    {
        var response = await _boardApi.DeleteAsync(boardId);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> InviteMemberAsync(Guid boardId, InviteMemberDto dto)
    {
        var response = await _boardApi.InviteMemberAsync(boardId, dto);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> RemoveMemberAsync(Guid boardId, Guid userId)
    {
        var response = await _boardApi.RemoveMemberAsync(boardId, userId);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> LeaveBoardAsync(Guid boardId)
    {
        var response = await _boardApi.LeaveBoardAsync(boardId);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }
        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> ToggleFavoriteAsync(Guid boardId)
    {
        var response = await _boardApi.ToggleFavoriteAsync(boardId);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> ArchiveBoardAsync(Guid boardId)
    {
        var respone = await _boardApi.ArchiveBoardAsync(boardId);

        if (respone.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(respone.GetErrorMessage());
    }

    public async Task<OperationResult> TransferOwnershipAsync(
        Guid boardId, TransferOwnershipDto dto)
    {
        var respone = await _boardApi.TransferOwnershipAsync(
            boardId, dto);

        if (respone.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(respone.GetErrorMessage());
    }

    public async Task<OperationResult> ChangeMemberRoleAsync(
        Guid boardId, Guid userId, ChangeMemberRoleDto dto)
    {
        var respone = await _boardApi.ChangeMemberRoleAsync(
            boardId, userId, dto);

        if (respone.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(respone.GetErrorMessage());
    }

    public async Task<List<BoardBackgroundDto>> GetBackgroundsAsync()
    {
        var response = await _boardApi.GetBackgroundsAsync();
        
        return response.IsSuccessStatusCode 
            && response.Content != null ? response.Content : [];
    }
}