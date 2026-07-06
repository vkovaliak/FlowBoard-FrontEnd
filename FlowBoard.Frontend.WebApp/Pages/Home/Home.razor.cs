using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authorization;
using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Services.Abstractions;
using MudBlazor;
using FlowBoard.Frontend.Domain.Models.Boards;
using FlowBoard.Frontend.WebApp.Components.Dialogs.CreateBoardDialog;
using FlowBoard.Frontend.Services.State;
using FlowBoard.Frontend.Domain.Models.Common;
using FlowBoard.Frontend.Services.Providers;

namespace FlowBoard.Frontend.WebApp.Pages.Home;

[Authorize]
public partial class Home
{
    [Inject] public IBoardService BoardService { get; set; } = default!;
    [Inject] public ICardService CardService { get; set; } = default!;
    [Inject] public IUserService UserService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public ISnackbar Snackbar { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public FavoritesState FavoritesState { get; set; } = default!;
    [Inject] public CustomAuthStateProvider AuthStateProvider { get; set; } = default!;

    private Guid _currentUserId;
    private string _userName = ""; 

    private IEnumerable<BoardDto>? _boards;
    private IEnumerable<BoardDto>? _archivedBoards;

    private int _boardsCount;
    private int _completedCount;
    private int _inProgressCount;
    private int _membersCount;

    protected override async Task OnInitializedAsync()
    {
        _currentUserId = await AuthStateProvider.GetCurrentUserIdAsync();
        
        _boards = await BoardService.GetMyBoardsAsync();
        _archivedBoards = await BoardService.GetArchivedBoardsAsync();

        var user = await UserService.GetMeAsync();
        _userName = user!.UserName;

        await LoadStatsAsync();
    }

    private async Task LoadStatsAsync()
    {
        var tasks = await CardService.GetMyTasksAsync();

        _boardsCount = _boards?.Count() ?? 0;
        _completedCount = tasks.Count(t => t.IsCompleted);
        _inProgressCount = tasks.Count(t => !t.IsCompleted);

        _membersCount = _boards?
            .SelectMany(b => b.Members)
            .Select(m => m.UserId)
            .Where(id => id != _currentUserId)
            .Distinct()
            .Count() ?? 0;
        }

    private Task OpenCreateBoardDialog() 
        => ManageBoardDialogAsync(null);

    private Task OpenEditBoardDialog(BoardDto board) 
        => ManageBoardDialogAsync(board);

    private async Task ManageBoardDialogAsync(BoardDto? currentBoard)
    {
        var options = new DialogOptions 
        { 
            CloseOnEscapeKey = true, 
            FullWidth = true,
            MaxWidth = MaxWidth.ExtraSmall 
        };

        var parameters = new DialogParameters<CreateBoardDialog>();
        if (currentBoard is not null)
        {
            parameters.Add(x => x.CurrentBoard, currentBoard);
        }

        var dialog = await DialogService.ShowAsync<CreateBoardDialog>(
            string.Empty, parameters, options);
        var dialogResult = await dialog.Result;

        if (dialogResult is { 
            Canceled: false, 
            Data: CreateBoardModel model 
            })
        {
            OperationResult<Guid> isSuccess;
            string successMessage;
            string errorMessage;

            if (currentBoard is null)
            {
                var createDto = new CreateBoardDto(
                    Name: model.Name, 
                    IsPublic: model.IsPublic,
                    Background: model.Background);

                isSuccess = await BoardService.CreateAsync(createDto);
                successMessage = "Board created!";
                NavigateToBoard(isSuccess.Value);
                errorMessage = $"Failed to create board: {isSuccess.Error}";
            }
            else 
            {
                var updateDto = new UpdateBoardDto(
                    Name: model.Name, 
                    IsPublic: model.IsPublic,
                    Background: model.Background
                );

                isSuccess = await BoardService.UpdateAsync(
                    currentBoard.Id, updateDto);
                successMessage = "Board updated successfully!";
                errorMessage = $"Failed to update board: {isSuccess.Error}";
            }

            if (isSuccess.Success)
            {
                Snackbar.Add(successMessage, Severity.Success);
                _boards = await BoardService.GetMyBoardsAsync();
            }
            else
            {
                Snackbar.Add(errorMessage, Severity.Error);
            }
        }
    }

    private async Task OpenDeleteConfirmation(BoardDto board)
    {
        bool? result = await DialogService.ShowMessageBoxAsync(
            "Delete Board", 
            $"Are you sure you want to delete board '{board.Name}'?", 
            yesText: "Delete", 
            cancelText: "Cancel");

        if (result == true)
        {   
            var isDeleted = await BoardService.DeleteAsync(board.Id);

            if (isDeleted.Success)
            {
                Snackbar.Add(
                    $"Board '{board.Name}' deleted", Severity.Success);
                _boards = await BoardService.GetMyBoardsAsync();
            }
            else
            {
                Snackbar.Add(isDeleted.Error ?? "Failed", Severity.Error);
            }
        }
    }

    private async Task OnRestoreBoard(BoardDto board)
    {
        Snackbar.Add("Restore will be available later", Severity.Info);
        await Task.CompletedTask;
    }

    private async Task OpenArchiveConfirmation(BoardDto board)
    {
        bool? result = await DialogService.ShowMessageBoxAsync(
            "Archive Board", 
            $"Are you sure you want to archive board '{board.Name}'?", 
            yesText: "Archive", 
            cancelText: "Cancel");

        if (result == true)
        {   
            var isArchived = await BoardService.ArchiveBoardAsync(board.Id);

            if (isArchived.Success)
            {
                Snackbar.Add($"Board '{board.Name}' archived", Severity.Success);
                _boards = await BoardService.GetMyBoardsAsync();
            }
            else
            {
                Snackbar.Add(isArchived.Error ?? "Failed", Severity.Error);
            }
        }
    }

    private async Task ToggleFavoriteAsync(BoardDto board)
    {
        var result = await BoardService.ToggleFavoriteAsync(board.Id);

        if (result.Success)
        {
            _boards = await BoardService.GetMyBoardsAsync();
            FavoritesState.NotifyChanged();
        }
        else
        {
            Snackbar.Add(result.Error ?? "Failed", Severity.Error);
        }
    }

    private void NavigateToBoard(Guid boardId)
    {
        NavigationManager.NavigateTo($"/boards/{boardId}");
    }
}