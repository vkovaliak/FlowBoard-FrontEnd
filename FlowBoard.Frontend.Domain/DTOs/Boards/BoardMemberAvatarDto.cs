namespace FlowBoard.Frontend.Domain.DTOs.Boards;

public record BoardMemberAvatarDto(
    Guid UserId,
    string UserName,
    string? AvatarUrl);