using FlowBoard.Frontend.Domain.DTOs.Notifications;
using Refit;

namespace FlowBoard.Frontend.Services.Http;

public interface INotificationApi
{
    [Get("/api/notifications")]
    Task<ApiResponse<List<NotificationDto>>> GetMyNotificationsAsync();

    [Get("/api/notifications/unread-count")]
    Task<ApiResponse<int>> GetUnreadCountAsync();

    [Put("/api/notifications/{notificationId}/read")]
    Task<ApiResponse<bool>> MarkAsReadAsync(Guid notificationId);

    [Put("/api/notifications/read-all")]
    Task<ApiResponse<bool>> MarkAllAsReadAsync();
}