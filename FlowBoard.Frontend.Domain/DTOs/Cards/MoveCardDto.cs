namespace FlowBoard.Frontend.Domain.DTOs.Cards;

public record MoveCardDto(
    Guid NewListId,
    int NewPosition);