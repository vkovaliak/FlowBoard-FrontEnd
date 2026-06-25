using FlowBoard.Frontend.Domain.DTOs.Cards;
using FlowBoard.Frontend.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Pages;

[Authorize]
public partial class MyTasks
{
    [Inject] public ICardService CardService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    private bool _loading = true;

    private List<MyCardDto> _allTasks = [];
    private List<MyCardDto> _overdue = [];
    private List<MyCardDto> _dueToday = [];
    private List<MyCardDto> _upcoming = [];
    private List<MyCardDto> _noDate = [];

    private int _thisWeekCount;
    private int _completedCount;

    protected override async Task OnInitializedAsync()
    {
        await LoadTasksAsync();
    }

    private async Task LoadTasksAsync()
    {
        _loading = true;

        _allTasks = await CardService.GetMyTasksAsync();
        GroupTasks();

        _loading = false;
    }

    private void GroupTasks()
    {
        var today = DateTime.Today;
        var weekEnd = today.AddDays(7);

        var active = _allTasks.Where(t => !t.IsCompleted)
            .ToList();

        _overdue = active
            .Where(t => t.DueDate.HasValue 
                && t.DueDate.Value.Date < today)
            .OrderBy(t => t.DueDate)
            .ToList();

        _dueToday = active
            .Where(t => t.DueDate.HasValue 
                && t.DueDate.Value.Date == today)
            .ToList();

        _upcoming = active
            .Where(t => t.DueDate.HasValue 
                && t.DueDate.Value.Date > today)
            .OrderBy(t => t.DueDate)
            .ToList();

        _noDate = active
            .Where(t => !t.DueDate.HasValue)
            .ToList();

        _thisWeekCount = active
            .Count(t => t.DueDate.HasValue
                && t.DueDate.Value.Date >= today
                && t.DueDate.Value.Date <= weekEnd);

        _completedCount = _allTasks.Count(t => t.IsCompleted);
    }

    private async Task OpenBoardAsync(MyCardDto task)
    {
        NavigationManager.NavigateTo($"boards/{task.BoardId}");
    }
}