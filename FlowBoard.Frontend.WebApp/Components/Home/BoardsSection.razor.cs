using FlowBoard.Frontend.Domain.DTOs.Boards;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.Home;

public partial class BoardsSection
{
    [Parameter] public IEnumerable<BoardDto>? Boards { get; set; }
    [Parameter] public IEnumerable<BoardDto>? ArchivedBoards { get; set; }
    [Parameter] public Guid CurrentUserId { get; set; }

    private IEnumerable<BoardDto>? StarredBoards =>
        Boards?.Where(b => b.IsFavorite);

    [Parameter] public EventCallback OnCreateBoard { get; set; }
    [Parameter] public EventCallback<Guid> OnClick { get; set; }
    [Parameter] public EventCallback<BoardDto> OnToggleFavorite { get; set; }
    [Parameter] public EventCallback<BoardDto> OnEdit { get; set; }
    [Parameter] public EventCallback<BoardDto> OnArchive { get; set; }
    [Parameter] public EventCallback<BoardDto> OnRestore { get; set; }
    [Parameter] public EventCallback<BoardDto> OnDelete { get; set; }
}