using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.State;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Layout;

public partial class NavMenu : IDisposable
{
    [Inject] public IBoardService BoardService { get; set; } = default!;
    [Inject] public FavoritesState FavoritesState { get; set; } = default!;
    [Inject] public TasksState TasksState { get; set; } = default!;
    [Inject] public ICardService CardService { get; set; } = default!;

    private List<BoardDto> _favorites = [];
    private int _myTasksCount;

    protected override async Task OnInitializedAsync()
    {
        FavoritesState.OnChanged += HandleFavoritesChanged;
        TasksState.OnChanged += HandleTaskChanged;
        await LoadFavoritesAsync();
        await LoadMyTasksCountAsync();
    }

    private async Task LoadMyTasksCountAsync()
    {
        var tasks = await CardService.GetMyTasksAsync();
        _myTasksCount = tasks.Count(c => !c.IsCompleted);
    }

    private async Task LoadFavoritesAsync()
    {
        var boards = await BoardService.GetMyBoardsAsync();
        _favorites = boards
            .Where(b => b.IsFavorite)
            .ToList();
    }

    private async void HandleFavoritesChanged()
    {
        await LoadFavoritesAsync();
        await InvokeAsync(StateHasChanged);
    }

    private async void HandleTaskChanged()
    {
        await LoadMyTasksCountAsync();
        await InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        FavoritesState.OnChanged -= HandleFavoritesChanged;
        TasksState.OnChanged -= HandleTaskChanged;
    }
}