using FlowBoard.Frontend.Domain.DTOs.Boards;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.Home;

public partial class MemberAvatars
{
    [Parameter, EditorRequired]
    public List<BoardMemberAvatarDto> Members { get; set; } = [];

    [Parameter] public int MaxVisible { get; set; } = 3;

    private IEnumerable<BoardMemberAvatarDto> Visible 
        => Members.Take(MaxVisible);

    private int Overflow 
        => Members.Count > MaxVisible ? Members.Count - MaxVisible : 0;
}