using FlowBoard.Frontend.Domain.DTOs.Cards;
using FlowBoard.Frontend.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Pages.Calendar;

[Authorize]
public partial class Calendar
{
    [Inject] public ICardService CardService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    private bool _loading = true;
    private List<MyCardDto> _tasks = [];
    private DateTime _currentMonth = DateTime.Today;

    protected override async Task OnInitializedAsync()
    {
        await LoadTasksAsync();
    }

    private async Task LoadTasksAsync()
    {
        _loading = true;
        var allTasks = await CardService.GetMyTasksAsync();

        _tasks = allTasks
            .Where(t => !t.IsCompleted)
            .ToList();

        _loading = false;
    }

    private void GoToday() 
        => _currentMonth = DateTime.Today;

    private void PreviousMonth()
        => _currentMonth = _currentMonth.AddMonths(-1);

    private void NextMonth()
        => _currentMonth = _currentMonth.AddMonths(1);

    private async Task OpenCardAsync(MyCardDto task)
    {
        NavigationManager.NavigateTo($"boards/{task.BoardId}");
    }
}