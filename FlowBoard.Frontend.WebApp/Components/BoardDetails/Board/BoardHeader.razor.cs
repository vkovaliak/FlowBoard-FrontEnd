using FlowBoard.Frontend.Domain.Authorization;
using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Domain.Enums;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.BoardDetails.Board;

public partial class BoardHeader
{
    [Parameter] public string BoardName { get; set; } = string.Empty;
    [Parameter] public bool IsPublic { get; set; }
    [Parameter] public List<BoardMemberDto> Members { get; set; } = [];
    [Parameter] public Guid BoardId { get; set; }
    [Parameter] public Guid CreatedBy { get; set; }
    [Parameter] public Guid CurrentUserId { get; set; }
    [Parameter] public BoardRole CurrentUserRole { get; set; }
    [Parameter] public BoardViewTab ActiveTab { get; set; }
    [Parameter] public EventCallback<BoardViewTab> OnTabChanged { get; set; }
    [Parameter] public EventCallback OnMembersChanged { get; set; }
    [Parameter] public bool IsFavorite { get; set; }
    [Parameter] public EventCallback OnToggleFavorite { get; set; }
    [Parameter] public EventCallback OnInviteClick { get; set; }
    [Parameter] public EventCallback OnMeetClick { get; set; }

    private bool CanInvite 
        => BoardPermissions.CanInviteMembers(CurrentUserRole);
}