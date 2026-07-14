using FlowBoard.Frontend.Domain.DTOs.Cards;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.BoardDetails.Calendar;

public partial class BoardCalendarTaskItem
{
    [Parameter] public CardDto Card { get; set; } = default!;
    [Parameter] public EventCallback<CardDto> OnClick { get; set; }
}