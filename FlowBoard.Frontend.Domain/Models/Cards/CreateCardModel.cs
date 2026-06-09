namespace FlowBoard.Frontend.Domain.Models.Cards;

public class CreateCardModel
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}