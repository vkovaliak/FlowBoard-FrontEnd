using FlowBoard.Frontend.Domain.Constants;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Configurations;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

namespace FlowBoard.Frontend.Services.Implementations;

public class NotificationHubService : INotificationHubService
{
    private readonly HubConnection _connection;

    public event Action? OnNotificationReceived;

    public NotificationHubService(
        IOptions<ApiOptions> apiOptions,
        ITokenService tokenService)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl($"{apiOptions.Value.BaseUrl}{HubRoutes.Notifications}",
                options =>
                {
                    options.AccessTokenProvider =
                        async () => await tokenService.GetAccessTokenAsync();
                })
            .WithAutomaticReconnect()
            .Build();

        _connection.On(
            HubMethods.NotificationReceived,
            () =>
            {
                OnNotificationReceived?.Invoke();
            });
    }

    public async Task ConnectAsync()
    {
        if (_connection.State == HubConnectionState.Disconnected)
        {
            await _connection.StartAsync();
        }
    }
}