using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.State;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Layout;

public partial class NavMenu : IDisposable
{
    [Inject] public IBoardService BoardService { get; set; } = default!;
    [Inject] public FavoritesState FavoritesState { get; set; } = default!;

    private List<BoardDto> _favorites = [];

    protected override async Task OnInitializedAsync()
    {
        FavoritesState.OnChanged += HandleFavoritesChanged;
        await LoadFavoritesAsync();
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

    public void Dispose()
    {
        FavoritesState.OnChanged -= HandleFavoritesChanged;
    }
}