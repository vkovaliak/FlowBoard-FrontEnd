using FlowBoard.Frontend.Domain.DTOs.Cards;

namespace FlowBoard.Frontend.Domain.Models.Boards;

public class TimelineRowModel
{
    public CardDto Card { get; set; } = default!;

    public Guid ListId { get; set; }
}