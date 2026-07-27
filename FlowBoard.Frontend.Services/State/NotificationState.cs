using FlowBoard.Frontend.Domain.DTOs.Notifications;
using FlowBoard.Frontend.Services.Abstractions;

namespace FlowBoard.Frontend.Services.State;

public class NotificationState
{
    private readonly INotificationService _service;
    private readonly INotificationHubService _hub;

    public List<NotificationDto> Notifications { get; private set; } = [];
    public int UnreadCount { get; private set; }

    public event Action? OnChange;

    public NotificationState(
        INotificationService service,
        INotificationHubService hub)
    {
        _service = service;
        _hub = hub;

        _hub.OnNotificationReceived += HandleReceived;
    }

    public async Task InitializeAsync()
    {
        await _hub.ConnectAsync();
        await ReloadAsync();
    }

    private async void HandleReceived()
    {
        await ReloadAsync();
    }

    public async Task ReloadAsync()
    {
        Notifications = await _service.GetMyNotificationsAsync();

        var count = await _service.GetUnreadCountAsync();
        
        UnreadCount = count.Value;
        OnChange?.Invoke();
    }

    public async Task MarkAsReadAsync(Guid notificationId)
    {
        await _service.MarkAsReadAsync(notificationId);
        await ReloadAsync();
    }

    public async Task MarkAllAsReadAsync()
    {
        await _service.MarkAllAsReadAsync();
        await ReloadAsync();
    }
}