namespace FlowBoard.Frontend.Domain.Models.Boards;

public class CreateBoardModel
{
    public string Name { get; set; } = string.Empty;
    public bool IsPublic { get; set; } = true;
}