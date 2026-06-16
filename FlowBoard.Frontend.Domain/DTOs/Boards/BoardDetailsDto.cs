using FlowBoard.Frontend.Domain.DTOs.Lists;
using FlowBoard.Frontend.Domain.Enums;

namespace FlowBoard.Frontend.Domain.DTOs.Boards;

public record BoardDetailsDto(
    Guid Id,
    string Name,
    bool IsPublic,
    Guid CreatedBy,
    DateTime CreatedAt,
    BoardRole UserRole,
    List<ListDto> Lists,
    List<BoardMemberDto> Members
);