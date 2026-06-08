using FlowBoard.Frontend.Domain.DTOs.Lists;

namespace FlowBoard.Frontend.Domain.DTOs.Boards;

public record BoardDetailsDto(
    Guid Id,
    string Name,
    bool IsPublic,
    Guid CreatedBy,
    DateTime CreatedAt,
    List<ListDto> Lists
);