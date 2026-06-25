using FlowBoard.Frontend.Domain.DTOs.Cards;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.MyTasks;

public partial class MyTasksGroup
{
    [Parameter] public string Title { get; set; } = string.Empty;
    [Parameter] public string Icon { get; set; } = Icons.Material.Filled.List;
    [Parameter] public Color IconColor { get; set; } = Color.Default;
    [Parameter] public List<MyCardDto> Tasks { get; set; } = [];
    [Parameter] public EventCallback<MyCardDto> OnTaskClick { get; set; }
}