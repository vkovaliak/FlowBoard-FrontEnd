using FlowBoard.Frontend.Domain.DTOs.Boards;
using Refit;

namespace FlowBoard.Frontend.Services.Http;

public interface IBoardApi
{
    [Get("/api/boards")]
    Task<ApiResponse<IEnumerable<BoardDto>>> GetMyBoardsAsync();

    [Post("/api/boards")]
    Task<ApiResponse<Guid>> CreateAsync([Body] CreateBoardDto dto);
}