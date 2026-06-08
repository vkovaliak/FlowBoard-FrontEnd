namespace FlowBoard.Frontend.Domain.DTOs.Cards;

public record CardDto(
    Guid Id,
    Guid ListId,
    string Name,
    string? Description,
    int Position
);