using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Configurations;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using FlowBoard.Frontend.Domain.Constants;

namespace FlowBoard.Frontend.Services.Implementations;

public class CommentHubService : ICommentHubService
{
    private readonly HubConnection _connection;

    public event Action<Guid>? OnCommentCreated;

    public CommentHubService(
        IOptions<ApiOptions> apiOptions)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl($"{apiOptions.Value.BaseUrl}{HubRoutes.Comments}")
            .WithAutomaticReconnect()
            .Build();

         _connection.On<Guid>(
            HubMethods.ReceiveNewComment,
            commentId =>
            {
                OnCommentCreated?.Invoke(commentId);
            });
    }

    public async Task ConnectAsync()
    {
        if (_connection.State == HubConnectionState.Disconnected)
        {
            await _connection.StartAsync();
        }
    }

    public async Task JoinCardCommentsAsync(Guid cardId)
    {
        await _connection.InvokeAsync(
            HubClientMethods.JoinCardComments,
            cardId);
    }

    public async Task LeaveCardCommentsAsync(Guid cardId)
    {
        await _connection.InvokeAsync(
            HubClientMethods.LeaveCardComments,
            cardId);
    }
}