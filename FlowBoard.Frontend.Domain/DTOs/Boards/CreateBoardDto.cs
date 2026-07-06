namespace FlowBoard.Frontend.Domain.DTOs.Boards;

public record CreateBoardDto(
    string Name,
    bool IsPublic,
    string? Background);