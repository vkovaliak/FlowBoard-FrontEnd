using FlowBoard.Frontend.Domain.DTOs.Cards;

namespace FlowBoard.Frontend.Domain.DTOs.Lists;

public record ListDto(
    Guid Id,
    Guid BoardId,
    string Name,
    int Position,
    List<CardDto> Cards
);