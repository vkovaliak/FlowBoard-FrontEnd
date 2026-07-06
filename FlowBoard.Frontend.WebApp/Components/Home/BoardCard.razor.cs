using FlowBoard.Frontend.Domain.Authorization;
using FlowBoard.Frontend.Domain.DTOs.Boards;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.Home;

public partial class BoardCard
{
    [Parameter, EditorRequired] public BoardDto Board { get; set; } = default!;
    [Parameter] public bool IsArchived { get; set; } 
    [Parameter] public Guid CurrentUserId { get; set; }

    [Parameter] public EventCallback<Guid> OnClick { get; set; }
    [Parameter] public EventCallback<BoardDto> OnToggleFavorite { get; set; }
    [Parameter] public EventCallback<BoardDto> OnEdit { get; set; }
    [Parameter] public EventCallback<BoardDto> OnArchive { get; set; }
    [Parameter] public EventCallback<BoardDto> OnRestore { get; set; }
    [Parameter] public EventCallback<BoardDto> OnDelete { get; set; }

    private bool CanManage => BoardPermissions.CanManageBoard(Board.UserRole);

    private bool HasMenuActions => IsArchived || CanManage;

    private string CardStyle => IsArchived
        ? "cursor: default; min-height: 120px;"
        : "cursor: pointer; min-height: 120px;";

    private async Task HandleCardClick()
    {
        if (IsArchived)
        {
            return;
        }

        await OnClick.InvokeAsync(Board.Id);
    }
}