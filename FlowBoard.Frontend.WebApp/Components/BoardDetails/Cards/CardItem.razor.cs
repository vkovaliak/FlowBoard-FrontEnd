using FlowBoard.Frontend.Domain.DTOs.Cards;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.BoardDetails.Cards;

public partial class CardItem
{
    [Parameter] 
    public CardDto Card { get; set; } = default!;
    
    [Parameter] 
    public EventCallback<CardDto> OnEdit { get; set; }

    [Parameter] 
    public EventCallback<CardDto> OnDelete { get; set; }

    [Parameter]
    public EventCallback<CardDto> OnToggleComplete { get; set; }

    private string GetTitleStyle()
    {
        var baseStyle = "word-break: break-word;";

        return Card.IsCompleted
            ? $"{baseStyle} text-decoration: line-through; color: #97a0af;"
            : baseStyle;
    }

    private Color GetDueDateColor()
    {
        if (Card.IsCompleted)
        {
            return Color.Success;
        }

        if (Card.DueDate < DateTime.Today)
        {
            return Color.Error;
        }

        if (Card.DueDate == DateTime.Today)
        {
            return Color.Warning;
        }

        return Color.Default;
    }
}