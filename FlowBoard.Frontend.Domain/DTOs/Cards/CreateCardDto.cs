namespace FlowBoard.Frontend.Domain.DTOs.Cards;

public record CreateCardDto(
    Guid ListId,
    Guid BoardId,
    string Name,
    string? Description);