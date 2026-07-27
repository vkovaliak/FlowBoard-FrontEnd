namespace FlowBoard.Frontend.Domain.Constants;

public static class HubRoutes
{
    public const string Comments = "/hubs/comments";
    public const string Board = "/hubs/boards";
    public const string Notifications = "/hubs/notifications";
}

public static class HubMethods
{
    public const string CommentUpdated = "CommentUpdated";
    public const string BoardUpdated = "BoardUpdated";
    public const string NotificationReceived = "NotificationReceived";
}

public static class HubClientMethods
{
    public const string JoinCardComments = "JoinCardComments";
    public const string LeaveCardComments = "LeaveCardComments";

    public const string JoinBoard = "JoinBoard";
    public const string LeaveBoard = "LeaveBoard";
}