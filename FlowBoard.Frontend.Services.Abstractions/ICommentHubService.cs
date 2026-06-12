using FlowBoard.Frontend.Domain.DTOs.Comments;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface ICommentHubService
{
    Task ConnectAsync();

    Task JoinCardCommentsAsync(Guid cardId);

    Task LeaveCardCommentsAsync(Guid cardId);

    event Action<Guid> OnCommentUpdated;
}