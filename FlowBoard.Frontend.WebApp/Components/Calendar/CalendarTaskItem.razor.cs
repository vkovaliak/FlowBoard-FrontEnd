using FlowBoard.Frontend.Domain.DTOs.Cards;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.Calendar;

public partial class CalendarTaskItem
{
    [Parameter] public MyCardDto Task { get; set; } = default!;
    [Parameter] public EventCallback<MyCardDto> OnClick { get; set; }
}