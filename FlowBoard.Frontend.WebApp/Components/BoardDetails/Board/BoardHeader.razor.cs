using FlowBoard.Frontend.Domain.Authorization;
using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Domain.Enums;
using FlowBoard.Frontend.Services.Handlers;
using FlowBoard.Frontend.Services.State;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.BoardDetails.Board;

public partial class BoardHeader : IDisposable
{
    [Inject] private UserState UserState { get; set; } = default!;
    [Inject] private UpgradeHandler UpgradeHandler { get; set; } = default!;
    
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
    [Parameter] public EventCallback OnArchiveClick { get; set; }

    private bool IsOwner => CurrentUserRole == BoardRole.Owner;

    private bool CanInvite 
        => BoardPermissions.CanInviteMembers(CurrentUserRole);

    private bool IsPro => UserState.IsPro;

    protected override async Task OnInitializedAsync()
    {
        await UserState.EnsureLoadedAsync();
        UserState.OnChanged += OnStateChanged;
    }

    private async void OnStateChanged()
        => await InvokeAsync(StateHasChanged);

    private async Task UpgradeAsync()
        => await UpgradeHandler.StartUpgradeAsync();

    public void Dispose()
        => UserState.OnChanged -= OnStateChanged;
}