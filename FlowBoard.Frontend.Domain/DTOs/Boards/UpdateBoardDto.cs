namespace FlowBoard.Frontend.Domain.DTOs.Boards;

public record UpdateBoardDto(
    string Name, 
    bool IsPublic,
    string? Background);