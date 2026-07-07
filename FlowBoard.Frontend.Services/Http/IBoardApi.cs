using FlowBoard.Frontend.Domain.DTOs.Boards;
using Refit;

namespace FlowBoard.Frontend.Services.Http;

public interface IBoardApi
{
    [Get("/api/boards")]
    Task<ApiResponse<IEnumerable<BoardDto>>> GetMyBoardsAsync();

    [Get("/api/boards/archived")]
    Task<ApiResponse<IEnumerable<BoardDto>>> GetArchivedBoardsAsync();

    [Get("/api/boards/{id}")]
    Task<ApiResponse<BoardDetailsDto>> GetDetailsAsync(Guid id);

    [Post("/api/boards")]
    Task<ApiResponse<Guid>> CreateAsync([Body] CreateBoardDto dto);

    [Put("/api/boards/{id}")]
    Task<ApiResponse<Guid>> UpdateAsync(Guid id, [Body] UpdateBoardDto dto);

    [Delete("/api/boards/{id}")]
    Task<ApiResponse<bool>> DeleteAsync(Guid id);

    [Post("/api/boards/{id}/invite")]
    Task<ApiResponse<bool>> InviteMemberAsync(Guid id, [Body] InviteMemberDto dto);

    [Delete("/api/boards/{boardId}/members/{userId}")]
    Task<ApiResponse<bool>> RemoveMemberAsync(Guid boardId, Guid userId);

    [Delete("/api/boards/{boardId}/leave")]
    Task<ApiResponse<bool>> LeaveBoardAsync(Guid boardId);

    [Put("/api/boards/{boardId}/favorite")]
    Task<ApiResponse<bool>> ToggleFavoriteAsync(Guid boardId);

    [Patch("/api/boards/{boardId}/archive")]
    Task<ApiResponse<bool>> ArchiveBoardAsync(Guid boardId);

    [Post("/api/boards/{boardId}/restore")]
    Task<ApiResponse<bool>> RestoreBoardAsync(Guid boardId);

    [Post("/api/boards/{boardId}/transfer-ownership")]
    Task<ApiResponse<bool>> TransferOwnershipAsync(
        Guid boardId, TransferOwnershipDto dto);

    [Patch("/api/boards/{boardId}/members/{userId}/role")]
    Task<ApiResponse<bool>> ChangeMemberRoleAsync(
        Guid boardId, Guid userId, ChangeMemberRoleDto dto);

    [Get("/api/boards/backgrounds")]
    Task<ApiResponse<List<BoardBackgroundDto>>> GetBackgroundsAsync();

}