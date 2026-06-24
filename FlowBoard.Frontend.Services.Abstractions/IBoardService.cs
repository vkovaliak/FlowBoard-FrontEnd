using FlowBoard.Frontend.Domain.DTOs.Boards;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface IBoardService
{
    Task<IEnumerable<BoardDto>> GetMyBoardsAsync();
    Task<BoardDetailsDto?> GetDetailsAsync(Guid boardId);
    Task<bool> CreateAsync(CreateBoardDto dto);
    Task<bool> UpdateAsync(Guid boardId, UpdateBoardDto dto);
    Task<bool> DeleteAsync(Guid boardId);
    Task<bool> InviteMemberAsync(Guid boardId, InviteMemberDto dto);
    Task<bool> RemoveMemberAsync(Guid boardId, Guid userId);
    Task<bool> LeaveBoardAsync(Guid boardId);
}