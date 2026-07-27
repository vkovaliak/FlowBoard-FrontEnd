using FlowBoard.Frontend.Domain.DTOs.Notifications;
using FlowBoard.Frontend.Domain.Models.Common;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Helpers;
using FlowBoard.Frontend.Services.Http;

namespace FlowBoard.Frontend.Services.Implementations;

public class NotificationService : INotificationService
{
    private readonly INotificationApi _notificationApi;

    public NotificationService(INotificationApi notificationApi)
    {
        _notificationApi = notificationApi;
    }

     public async Task<List<NotificationDto>> GetMyNotificationsAsync()
    {
        var response = await _notificationApi.GetMyNotificationsAsync();

        return response.IsSuccessStatusCode 
            && response.Content != null ? response.Content : [];
    }

    public async Task<OperationResult<int>> GetUnreadCountAsync()
    {
        var response = await _notificationApi.GetUnreadCountAsync();

        if (response.IsSuccessStatusCode)
        {
            return OperationResult<int>.Ok(response.Content);
        }

        return OperationResult<int>.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> MarkAsReadAsync(Guid notificationId)
    {
        var response = await _notificationApi.MarkAsReadAsync(notificationId);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> MarkAllAsReadAsync()
    {
        var response = await _notificationApi.MarkAllAsReadAsync();

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

}