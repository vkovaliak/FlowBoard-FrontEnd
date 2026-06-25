using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.Calendar;

public partial class CalendarToolbar
{
    [Parameter] public DateTime CurrentMonth { get; set; }
    [Parameter] public string ActiveView { get; set; } = "Month";
    [Parameter] public EventCallback OnToday { get; set; }
    [Parameter] public EventCallback OnPrevious { get; set; }
    [Parameter] public EventCallback OnNext { get; set; }
    [Parameter] public EventCallback<string> OnViewChange { get; set; }

    private readonly string[] _views = ["Month", "Week", "Day"];

    private string GetViewStyle(string view)
    {
        var isActive = view == ActiveView;

        var baseStyle =
            "padding: 8px 15px; border-radius: 6px; cursor: pointer; " +
            "font-size: 18px; font-weight: 500; transition: all 0.15s; " +
            "user-select: none;";

        if (isActive)
        {
            return baseStyle +
                "background-color: #695cfe1f; " +
                "color: var(--mud-palette-primary); " +
                "box-shadow: 0 1px 3px rgba(0,0,0,0.1);";
        }

        return baseStyle +
            "background-color: white; " +
            "color: var(--mud-palette-text-secondary);";
    }
}