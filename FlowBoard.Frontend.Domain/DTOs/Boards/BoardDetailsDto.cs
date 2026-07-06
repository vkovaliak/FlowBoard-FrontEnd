using FlowBoard.Frontend.Domain.DTOs.Lists;
using FlowBoard.Frontend.Domain.Enums;

namespace FlowBoard.Frontend.Domain.DTOs.Boards;

public record BoardDetailsDto(
    Guid Id,
    string Name,
    bool IsPublic,
    string? Background,
    Guid CreatedBy,
    DateTime CreatedAt,
    bool IsFavorite,
    BoardRole UserRole,
    bool OwnerIsPro,
    List<ListDto> Lists,
    List<BoardMemberDto> Members
);