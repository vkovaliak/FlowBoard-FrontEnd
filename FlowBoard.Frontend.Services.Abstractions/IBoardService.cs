using FlowBoard.Frontend.Domain.DTOs.Boards;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface IBoardService
{
    Task<IEnumerable<BoardDto>> GetMyBoardsAsync();
    Task<bool> CreateAsync(CreateBoardDto dto);
    Task<bool> UpdateAsync(UpdateBoardDto dto);
    Task<bool> DeleteAsync(DeleteBoardDto dto);
}