using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Domain.Models.Common;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface IBoardService
{
    Task<IEnumerable<BoardDto>> GetMyBoardsAsync();
    Task<IEnumerable<BoardDto>> GetArchivedBoardsAsync();
    Task<BoardDetailsDto?> GetDetailsAsync(Guid boardId);
    Task<OperationResult<Guid>> CreateAsync(CreateBoardDto dto);
    Task<OperationResult<Guid>> UpdateAsync(Guid boardId, UpdateBoardDto dto);
    Task<OperationResult> DeleteAsync(Guid boardId);
    Task<OperationResult> InviteMemberAsync(Guid boardId, InviteMemberDto dto);
    Task<OperationResult> RemoveMemberAsync(Guid boardId, Guid userId);
    Task<OperationResult> LeaveBoardAsync(Guid boardId);
    Task<OperationResult> ToggleFavoriteAsync(Guid boardId);
    Task<OperationResult> ArchiveBoardAsync(Guid boardId);
    Task<OperationResult> RestoreBoardAsync(Guid boardId);
    
    Task<OperationResult> TransferOwnershipAsync(
        Guid boardId, TransferOwnershipDto dto);

    Task<OperationResult> ChangeMemberRoleAsync(
        Guid boardId, Guid userId, ChangeMemberRoleDto dto);

    Task<List<BoardBackgroundDto>> GetBackgroundsAsync();
}