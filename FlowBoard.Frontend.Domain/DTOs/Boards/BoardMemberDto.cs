using FlowBoard.Frontend.Domain.Enums;

namespace FlowBoard.Frontend.Domain.DTOs.Boards;

public record BoardMemberDto(
    Guid UserId,
    string EmailAddress,
    BoardRole Role);