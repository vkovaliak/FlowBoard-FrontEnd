using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.Home;

public partial class StatsCards
{
    [Parameter] public int BoardsCount { get; set; }
    [Parameter] public int CompletedCount { get; set; }
    [Parameter] public int InProgressCount { get; set; }
    [Parameter] public int MembersCount { get; set; }
}