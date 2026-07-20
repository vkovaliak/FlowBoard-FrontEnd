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

    [Parameter]
    public EventCallback<CardDto> OnDuplicate { get; set; }

    [Parameter] 
    public bool CanEdit { get; set; } = true;

    private string GetTitleStyle()
    {
        var baseStyle = "word-break: break-word; font-size: 16px; font-weight:600";

        return baseStyle;
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