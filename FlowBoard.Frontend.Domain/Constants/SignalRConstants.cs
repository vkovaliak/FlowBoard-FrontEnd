namespace FlowBoard.Frontend.Domain.Constants;

public static class HubRoutes
{
    public const string Comments = "/hubs/comments";
}

public static class HubMethods
{
    public const string CommentUpdated = "CommentUpdated";
}

public static class HubClientMethods
{
    public const string JoinCardComments = "JoinCardComments";

    public const string LeaveCardComments = "LeaveCardComments";
}