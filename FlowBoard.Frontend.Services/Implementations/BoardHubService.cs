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

    public BoardHubService(IOptions<ApiOptions> apiOptions)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl($"{apiOptions.Value.BaseUrl}{HubRoutes.Board}")
            .WithAutomaticReconnect()
            .Build();

        _connection.On<Guid>(
            HubMethods.BoardUpdated,
            boardId =>
            {
                OnBoardUpdated?.Invoke(boardId);
            });
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