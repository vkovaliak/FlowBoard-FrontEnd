using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.BoardDetails.Calendar;

public partial class BoardCalendarToolbar
{
    [Parameter] public DateTime CurrentMonth { get; set; }
    [Parameter] public EventCallback OnToday { get; set; }
    [Parameter] public EventCallback OnPrevious { get; set; }
    [Parameter] public EventCallback OnNext { get; set; }
}