namespace FlowBoard.Frontend.Domain.DTOs.Boards;

public record UpdateBoardDto(
    Guid BoardId,
    string Name, 
    bool IsPublic);