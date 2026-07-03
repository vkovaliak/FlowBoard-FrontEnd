using FlowBoard.Frontend.Domain.Enums;

namespace FlowBoard.Frontend.Domain.Authorization;

public static class BoardPermissions
{
    public static bool CanManageBoard(BoardRole role) =>
        role == BoardRole.Owner;

    public static bool CanInviteMembers(BoardRole role) =>
        role is BoardRole.Owner or BoardRole.Admin;

    public static bool CanRemoveMembers(BoardRole role) =>
        role is BoardRole.Owner or BoardRole.Admin;

    public static bool CanModifyContent(BoardRole role) =>
        role != BoardRole.Viewer;

    public static bool CanManageRoles(BoardRole role) =>
        role is BoardRole.Owner or BoardRole.Admin;

    public static bool CanTransferOwnership(BoardRole role) =>
        role == BoardRole.Owner;

    public static bool CanLeaveBoard(BoardRole role) =>
        role != BoardRole.Owner;

    public static IReadOnlyList<BoardRole> AssignableRoles(
        BoardRole actorRole)=> actorRole switch
        {
            BoardRole.Owner =>
                [BoardRole.Admin, BoardRole.Member, BoardRole.Viewer],
            BoardRole.Admin =>
                [BoardRole.Member, BoardRole.Viewer],
            _ => []
        };

    public static bool CanModifyMember(
        BoardRole actorRole, BoardRole targetRole)
    {
        if (targetRole == BoardRole.Owner)
        {
            return false;
        }

        return actorRole switch
        {
            BoardRole.Owner => true,
            BoardRole.Admin => targetRole is BoardRole.Member 
                or BoardRole.Viewer,
            _ => false
        };
    }
}