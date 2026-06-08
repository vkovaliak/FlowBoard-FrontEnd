using FlowBoard.Frontend.Domain.DTOs.Lists;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components;

public partial class TaskList
{
    [Parameter]
    public ListDto List { get; set; } = default!;
}