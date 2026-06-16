using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using MudBlazor;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Domain.DTOs.Boards;

namespace FlowBoard.Frontend.WebApp.Pages.BoardDetails;

[Authorize]
public partial class BoardDetails : IAsyncDisposable
{
    [Parameter]
    public Guid Id { get; set; }

    [Inject] public IBoardService BoardService { get; set; } = default!;
    [Inject] public ICardService CardService { get; set; } = default!;
    [Inject] public IListService ListService { get; set; } = default!;
    [Inject] public ISnackbar Snackbar { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public IJSRuntime JsRuntime { get; set; } = default!;
    [Inject] public IBoardHubService BoardHub { get; set; } = default!;

    private BoardDetailsDto? _board;
    private bool _isNotFound = false;

    private DotNetObjectReference<BoardDetails>? _objRef;

    protected override async Task OnInitializedAsync()
    {
        await RefreshBoardAsync();

        if (_board is null)
        {
            _isNotFound = true;
            Snackbar.Add("Board not found or access denied.", Severity.Error);
        }
        else
        {
            BoardHub.OnBoardUpdated += HandleBoardHubUpdate;
            await BoardHub.ConnectAsync();
            await BoardHub.JoinBoardAsync(Id);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_board != null && _objRef is null)
        {
            _objRef = DotNetObjectReference.Create(this);
            await JsRuntime.InvokeVoidAsync("initKanbanSortable", _objRef);
        }
    }

    private async void HandleBoardHubUpdate(Guid updatedBoardId)
    {
        if (updatedBoardId == Id)
        {
            await RefreshBoardAsync();
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task RefreshBoardAsync()
    {
        _board = await BoardService.GetDetailsAsync(Id);
    }

    private void ShowResult(bool success, string successMsg, string errorMsg)
    {
        Snackbar.Add(
            success ? successMsg : errorMsg,
            success ? Severity.Success : Severity.Error);
    }

    private async Task<bool> ConfirmDeleteAsync(string title, string message)
    {
        return await DialogService.ShowMessageBoxAsync(
            title, message,
            yesText: "Delete",
            cancelText: "Cancel") == true;
    }

    public async ValueTask DisposeAsync()
    {
        _objRef?.Dispose();
        
        BoardHub.OnBoardUpdated -= HandleBoardHubUpdate;
        await BoardHub.LeaveBoardAsync(Id);
    }
}