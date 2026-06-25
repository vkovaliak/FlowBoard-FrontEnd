using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Services.Abstractions;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Layout;

public partial class NavMenu
{
    [Inject] public IBoardService BoardService { get; set; } = default!;

    private List<BoardDto> _favorites = [];

    protected override async Task OnInitializedAsync()
    {
        await LoadFavoritesAsync();
    }

    private async Task LoadFavoritesAsync()
    {
        var boards = await BoardService.GetMyBoardsAsync();
        _favorites = boards
            .Where(b => b.IsFavorite)
            .ToList();
    }
}