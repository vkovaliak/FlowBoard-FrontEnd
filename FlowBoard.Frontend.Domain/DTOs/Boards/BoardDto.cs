using FlowBoard.Frontend.Domain.Enums;

namespace FlowBoard.Frontend.Domain.DTOs.Boards;

public record BoardDto(
    Guid Id,
    string Name,
    bool IsPublic,
    string Background,
    Guid CreatedBy,
    DateTime CreatedAt,
    bool IsFavorite,
    BoardRole UserRole)
{
    public List<BoardMemberAvatarDto> Members { get; set; } = [];
};