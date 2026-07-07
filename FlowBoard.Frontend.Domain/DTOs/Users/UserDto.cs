using FlowBoard.Frontend.Domain.Enums;

namespace FlowBoard.Frontend.Domain.DTOs.Users;

public record UserDto(
    Guid Id,
    string EmailAddress,
    string UserName,
    string? AvatarUrl,
    SubscriptionPlan SubscriptionPlan);