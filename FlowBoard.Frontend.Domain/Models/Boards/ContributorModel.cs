namespace FlowBoard.Frontend.Domain.Models.Boards;

public class ContributorModel
{
    public string UserName { get; set; } = string.Empty;

    public string? AvatarUrl { get; set; }

    public int Count { get; set; }
}