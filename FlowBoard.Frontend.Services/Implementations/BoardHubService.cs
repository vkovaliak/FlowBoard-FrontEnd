using FlowBoard.Frontend.Domain.Constants;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Configurations;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

namespace FlowBoard.Frontend.Services.Implementations;

public class BoardHubService : IBoardHubService
{
    private readonly HubConnection _connection;

    public event Action<Guid>? OnBoardUpdated;
    public event Action<Guid>? OnUserOnline;
    public event Action<Guid>? OnUserOffline;
    public event Action<IReadOnlyCollection<Guid>>? OnOnlineUsers;

    public BoardHubService(
        IOptions<ApiOptions> apiOptions,
        ITokenService tokenService)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl($"{apiOptions.Value.BaseUrl}{HubRoutes.Board}",
                options =>
                {
                    options.AccessTokenProvider = async () =>
                        await tokenService.GetAccessTokenAsync();
                })
            .WithAutomaticReconnect()
            .Build();

        _connection.On<Guid>(
            HubMethods.BoardUpdated,
            boardId => OnBoardUpdated?.Invoke(boardId));

        _connection.On<Guid>(
            HubMethods.UserOnline,
            userId => OnUserOnline?.Invoke(userId));

        _connection.On<Guid>(
            HubMethods.UserOffline,
            userId => OnUserOffline?.Invoke(userId));

        _connection.On<IReadOnlyCollection<Guid>>(
            HubMethods.OnlineUsers,
            users => OnOnlineUsers?.Invoke(users));
    }

    public async Task ConnectAsync()
    {
        if (_connection.State == HubConnectionState.Disconnected)
        {
            await _connection.StartAsync();
        }
    }

    public async Task JoinBoardAsync(Guid boardId)
    {
        await _connection.InvokeAsync(
            HubClientMethods.JoinBoard,
            boardId);
    }

    public async Task LeaveBoardAsync(Guid boardId)
    {
        await _connection.InvokeAsync(
            HubClientMethods.LeaveBoard,
            boardId);
    }
}