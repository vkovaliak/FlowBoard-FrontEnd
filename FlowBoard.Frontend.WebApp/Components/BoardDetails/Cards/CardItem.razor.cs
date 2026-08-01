using FlowBoard.Frontend.Domain.Enums;
using FlowBoard.Frontend.Domain.DTOs.Cards;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.BoardDetails.Cards;

public partial class CardItem
{
    [Parameter] public CardDto Card { get; set; } = default!;
    [Parameter] public EventCallback<CardDto> OnEdit { get; set; }
    [Parameter] public EventCallback<CardDto> OnDelete { get; set; }
    [Parameter] public EventCallback<(CardDto Card, MouseEventArgs Args)> OnToggleComplete { get; set; }
    [Parameter] public EventCallback<CardDto> OnDuplicate { get; set; }
    [Parameter] public bool CanEdit { get; set; } = true;

    private const string CompletedBlue = "#2563EB";

    private bool IsFullCover =>
        Card.CoverMode == CardCoverMode.Full
        && !string.IsNullOrEmpty(Card.CoverColor);

    private async Task ToggleCompleteAsync(MouseEventArgs e)
    {
        await OnToggleComplete.InvokeAsync((Card, e));
    }

    private string GetCardClass()
    {
        var baseClass = "card-item";
        baseClass += IsFullCover ? " card-item-full-cover" : " bg-white";

        if (Card.IsCompleted)
        {
            baseClass += " card-completed";
        }

        return baseClass;
    }

    private string GetCardStyle()
    {
        var baseStyle = "border-radius: 8px; position: relative;" + 
                        "cursor: pointer; overflow: hidden;";

        if (IsFullCover)
        {
            baseStyle += $" background-color: {Card.CoverColor};";
        }

        return baseStyle;
    }

    private string GetContentClass() => "pa-3";

    private string GetMoreVertClass()
    {
        return IsFullCover
            ? "card-morevert card-morevert-oncover"
            : "card-morevert";
    }

    private string GetTitleStyle()
    {
        var baseStyle = "word-break: break-word;" + 
                        "font-size: 16px; font-weight: 600;";

        if (IsFullCover)
        {
            baseStyle += " color: #ffffff;" + 
                         "text-shadow: 0 1px 3px rgba(0,0,0,0.4);";
        }

        return baseStyle;
    }

    private Color GetCompleteColor()
    {
        return Color.Default;
    }

    private string GetCompleteIconStyle()
    {
        var baseStyle = "padding: 2px; opacity: 1;";

        if (Card.IsCompleted)
        {
            var color = IsFullCover ? "#ffffff" : CompletedBlue;
            baseStyle += $" color: {color};";
        }
        else if (IsFullCover)
        {
            baseStyle += " color: #ffffff;";
        }

        return baseStyle;
    }

    private string GetDueDateBadgeClass()
    {
        var baseClass = "card-due-badge";
        if (IsFullCover)
        {
            baseClass += " card-due-badge-oncover";
        }
        return baseClass;
    }

    private string GetDueDateTextStyle()
    {
        return IsFullCover
            ? "color: #172B4D; font-weight: 500;"
            : "color: #64748B;";
    }

    private string GetMetaIconStyle()
    {
        var color = IsFullCover ? "#ffffff" : "#64748B";
        var shadow = IsFullCover ? " filter: drop-shadow(0 1px 2px rgba(0,0,0,0.4));" : "";
        return $"font-size: 16px; color: {color};{shadow}";
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