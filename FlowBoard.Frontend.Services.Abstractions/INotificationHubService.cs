namespace FlowBoard.Frontend.Services.Abstractions;

public interface INotificationHubService
{
    Task ConnectAsync();
    event Action OnNotificationReceived;
}