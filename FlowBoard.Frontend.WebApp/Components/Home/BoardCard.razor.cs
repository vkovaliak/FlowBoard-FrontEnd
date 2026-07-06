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

    private bool HasBackground => !string.IsNullOrEmpty(Board.Background);

    private string CoverStyle => HasBackground
        ? $"background-image: url('{Board.Background}'); " +
          "background-size: 100% 100%; background-position: top left;"
        : "background: linear-gradient(135deg, #c7d2fe 0%, #e0e7ff 100%);";

    private async Task HandleCardClick()
    {
        if (IsArchived)
        {
            return;
        }

        await OnClick.InvokeAsync(Board.Id);
    }
}