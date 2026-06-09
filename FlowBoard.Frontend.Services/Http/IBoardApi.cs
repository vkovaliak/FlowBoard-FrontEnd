using FlowBoard.Frontend.Domain.DTOs.Boards;
using Refit;

namespace FlowBoard.Frontend.Services.Http;

public interface IBoardApi
{
    [Get("/api/boards")]
    Task<ApiResponse<IEnumerable<BoardDto>>> GetMyBoardsAsync();

    [Get("/api/boards/{id}")]
    Task<ApiResponse<BoardDetailsDto>> GetDetailsAsync(Guid id);

    [Post("/api/boards")]
    Task<ApiResponse<Guid>> CreateAsync([Body] CreateBoardDto dto);

    [Put("/api/boards/{id}")]
    Task<ApiResponse<bool>> UpdateAsync(Guid id, [Body] UpdateBoardDto dto);

    [Delete("/api/boards/{id}")]
    Task<ApiResponse<bool>> DeleteAsync(Guid id);

    [Post("/api/boards/{id}/invite")]
    Task<ApiResponse<bool>> InviteMemberAsync(Guid id, [Body] InviteMemberDto dto);
}