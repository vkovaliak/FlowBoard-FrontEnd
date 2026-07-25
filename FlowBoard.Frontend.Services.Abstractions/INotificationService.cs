using FlowBoard.Frontend.Domain.DTOs.Notifications;
using FlowBoard.Frontend.Domain.Models.Common;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface INotificationService
{
    Task<List<NotificationDto>> GetMyNotificationsAsync();
    Task<OperationResult<int>> GetUnreadCountAsync();
    Task<OperationResult> MarkAsReadAsync(Guid notificationId);
    Task<OperationResult> MarkAllAsReadAsync();
}