using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.BoardDetails.Dashboard;

public partial class DashboardStatCards
{
    [Parameter] public int TotalCount { get; set; }
    [Parameter] public int InProgressCount { get; set; }
    [Parameter] public int CompletedCount { get; set; }
    [Parameter] public int OverdueCount { get; set; }
}