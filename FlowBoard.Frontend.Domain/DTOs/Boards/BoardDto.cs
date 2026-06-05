namespace FlowBoard.Frontend.Domain.DTOs.Boards;

public record BoardDto(
    Guid Id,
    string Name,
    bool IsPublic,
    Guid CreatedBy,
    DateTime CreatedAt);