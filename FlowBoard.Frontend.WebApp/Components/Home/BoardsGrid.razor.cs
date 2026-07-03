using FlowBoard.Frontend.Domain.DTOs.Boards;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.Home;

public partial class BoardsGrid
{
    [Parameter] public IEnumerable<BoardDto>? Boards { get; set; }
    [Parameter] public string EmptyMessage { get; set; } = "No boards yet.";
    [Parameter] public bool IsArchived { get; set; } 
    [Parameter] public Guid CurrentUserId { get; set; }
    
    [Parameter] public EventCallback<Guid> OnClick { get; set; }
    [Parameter] public EventCallback<BoardDto> OnToggleFavorite { get; set; }
    [Parameter] public EventCallback<BoardDto> OnEdit { get; set; }
    [Parameter] public EventCallback<BoardDto> OnArchive { get; set; }
    [Parameter] public EventCallback<BoardDto> OnRestore { get; set; }
    [Parameter] public EventCallback<BoardDto> OnDelete { get; set; }
}
