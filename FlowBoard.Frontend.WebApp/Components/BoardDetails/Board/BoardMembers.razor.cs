using Microsoft.AspNetCore.Components;
using MudBlazor;
using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Domain.Enums;

namespace FlowBoard.Frontend.WebApp.Components.BoardDetails.Board;

public partial class BoardMembers
{
    [Parameter] public List<BoardMemberDto> Members { get; set; } = [];

    private static Color GetRoleColor(BoardRole role) => role switch
    {
        BoardRole.Owner => Color.Warning,
        BoardRole.Member => Color.Primary,
        BoardRole.Viewer => Color.Default,
        _ => Color.Default
    };
}