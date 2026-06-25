using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.MyTasks;

public partial class MyTasksStats
{
    [Parameter] public int DueTodayCount { get; set; }
    [Parameter] public int OverdueCount { get; set; }
    [Parameter] public int ThisWeekCount { get; set; }
    [Parameter] public int CompletedCount { get; set; }

    private static string GetIconClass(Color color) => color switch
    {
        Color.Primary => "stat-icon-primary",
        Color.Error => "stat-icon-error",
        Color.Info => "stat-icon-info",
        Color.Success => "stat-icon-success",
        _ => "stat-icon-primary"
    };
}