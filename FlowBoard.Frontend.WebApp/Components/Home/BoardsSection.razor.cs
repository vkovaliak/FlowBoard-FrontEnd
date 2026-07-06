using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Services.Handlers;
using FlowBoard.Frontend.Services.State;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.Home;

public partial class BoardsSection : IDisposable
{
    [Inject] private UserState UserState { get; set; } = default!;
    [Inject] private UpgradeHandler UpgradeHandler { get; set; } = default!;
    
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

    private const int FreeMaxBoards = 3;

    protected override async Task OnInitializedAsync()
    {
        await UserState.EnsureLoadedAsync();
        UserState.OnChanged += OnStateChanged;
    }

    private async void OnStateChanged()
        => await InvokeAsync(StateHasChanged);

    private async Task Upgrade()
        => await UpgradeHandler.StartUpgradeAsync();

    private int OwnedBoardsCount =>
        Boards?.Count(b => b.CreatedBy == CurrentUserId) ?? 0;

    private bool CanCreateBoard =>
        UserState.IsPro || OwnedBoardsCount < FreeMaxBoards;

    public void Dispose()
    {
        UserState.OnChanged -= OnStateChanged;
    }
}