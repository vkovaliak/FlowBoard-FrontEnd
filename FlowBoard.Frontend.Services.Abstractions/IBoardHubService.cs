namespace FlowBoard.Frontend.Services.Abstractions;

public interface IBoardHubService
{
    Task ConnectAsync();
    Task JoinBoardAsync(Guid boardId);
    Task LeaveBoardAsync(Guid boardId);
    event Action<Guid> OnBoardUpdated;
}