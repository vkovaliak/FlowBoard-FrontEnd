using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Domain.DTOs.Search;
using FlowBoard.Frontend.Domain.Enums;
using FlowBoard.Frontend.Domain.Models.Boards;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Handlers;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.InviteBoardDialog;

public partial class InviteMemberDialog
{
    [CascadingParameter] public IMudDialogInstance MudDialog { get; set; } = default!;
    [Inject] private ISearchService SearchService { get; set; } = default!;
    [Inject] private UpgradeHandler UpgradeHandler { get; set; } = default!;

    [Parameter] public List<BoardRole> AvailableRoles { get; set; } = [];
    [Parameter] public int CurrentMembersCount { get; set; }
    [Parameter] public bool IsOwnerPro { get; set; }

    private const int FreeMaxMembers = 5;

    private BoardRole _role;
    private UserSearchDto? _selectedUser;

    private bool LimitReached =>
        !IsOwnerPro && CurrentMembersCount >= FreeMaxMembers;

    protected override void OnInitialized()
    {
        _role = AvailableRoles.Contains(BoardRole.Member)
            ? BoardRole.Member
            : AvailableRoles.FirstOrDefault();
    }

    private void Submit()
    {
        if (LimitReached || _selectedUser is null)
        {
            return;
        }

        var dto = new InviteMemberDto(_selectedUser.EmailAddress, _role);
        MudDialog.Close(DialogResult.Ok(dto));
    }

    private async Task Upgrade()
        => await UpgradeHandler.StartUpgradeAsync();

    private async Task<IEnumerable<UserSearchDto>> SearchUsersAsync(
        string value, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return [];
        }

        return await SearchService.SearchUsersAsync(value);
    }

    private void Cancel() => MudDialog.Cancel();

    private static string GetRoleIcon(BoardRole role) => role switch
    {
        BoardRole.Admin => Icons.Material.Filled.AdminPanelSettings,
        BoardRole.Member => Icons.Material.Filled.Edit,
        BoardRole.Viewer => Icons.Material.Filled.Visibility,
        _ => Icons.Material.Filled.Person
    };

    private static string GetRoleDescription(BoardRole role) => role switch
    {
        BoardRole.Admin => "Can manage members and edit the board",
        BoardRole.Member => "Can view and edit the board content",
        BoardRole.Viewer => "Can only view the board",
        _ => string.Empty
    };

    private string RoleToString(BoardRole role) => role.ToString();
}