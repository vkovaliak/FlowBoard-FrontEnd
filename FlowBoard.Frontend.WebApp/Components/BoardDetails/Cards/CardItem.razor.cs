using FlowBoard.Frontend.Domain.DTOs.Cards;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.BoardDetails.Cards;

public partial class CardItem
{
    [Parameter] 
    public CardDto Card { get; set; } = default!;
    
    [Parameter] 
    public EventCallback<CardDto> OnEdit { get; set; }

    [Parameter] 
    public EventCallback<CardDto> OnDelete { get; set; }
}