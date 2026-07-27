using FlowBoard.Frontend.Domain.Enums;

namespace FlowBoard.Frontend.Domain.DTOs.Notifications;

public record NotificationDto(
    Guid Id,
    NotificationType Type,
    Guid? BoardId,
    Guid? CardId,
    string Message,
    bool IsRead,
    DateTime CreatedAt,
    Guid? TriggeredBy,
    string? TriggeredByName,
    string? TriggeredByAvatar
);