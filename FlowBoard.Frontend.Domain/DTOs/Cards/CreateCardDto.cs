namespace FlowBoard.Frontend.Domain.DTOs.Cards;

public record CreateCardDto(
    Guid ListId,
    string Name,
    string? Description);