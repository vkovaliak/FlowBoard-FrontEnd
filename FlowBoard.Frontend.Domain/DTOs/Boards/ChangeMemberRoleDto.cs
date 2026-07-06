using FlowBoard.Frontend.Domain.Enums;

namespace FlowBoard.Frontend.Domain.DTOs.Boards;

public record ChangeMemberRoleDto(
    BoardRole NewRole);