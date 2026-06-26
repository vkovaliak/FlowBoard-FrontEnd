using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authorization;
using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Services.Abstractions;
using MudBlazor;
using FlowBoard.Frontend.Domain.Models.Boards;
using FlowBoard.Frontend.WebApp.Components.Dialogs.CreateBoardDialog;
using FlowBoard.Frontend.Services.State;

namespace FlowBoard.Frontend.WebApp.Pages.Home;

[Authorize]
public partial class Home
{
    [Inject] public IBoardService BoardService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public ISnackbar Snackbar { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public FavoritesState FavoritesState { get; set; } = default!;

    private IEnumerable<BoardDto>? _boards;

    protected override async Task OnInitializedAsync()
    {
        _boards = await BoardService.GetMyBoardsAsync();
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
            bool isSuccess;
            string successMessage;
            string errorMessage;

            if (currentBoard is null)
            {
                var createDto = new CreateBoardDto(
                    Name: model.Name, 
                    IsPublic: model.IsPublic);

                isSuccess = await BoardService.CreateAsync(createDto);
                successMessage = "Board created!";
                errorMessage = "Board failed to create";
            }
            else 
            {
                var updateDto = new UpdateBoardDto(
                    Name: model.Name, 
                    IsPublic: model.IsPublic);

                isSuccess = await BoardService.UpdateAsync(currentBoard.Id, updateDto);
                successMessage = "Board updated successfully!";
                errorMessage = "Failed to update board";
            }

            if (isSuccess)
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

            if (isDeleted)
            {
                Snackbar.Add($"Board '{board.Name}' deleted", Severity.Success);
                _boards = await BoardService.GetMyBoardsAsync();
            }
            else
            {
                Snackbar.Add("Failed to delete board", Severity.Error);
            }
        }
    }

    private async Task ToggleFavoriteAsync(BoardDto board)
    {
        var success = await BoardService.ToggleFavoriteAsync(board.Id);

        if (success)
        {
            _boards = await BoardService.GetMyBoardsAsync();
            FavoritesState.NotifyChanged();
        }
        else
        {
            Snackbar.Add("Failed to update favorite", Severity.Error);
        }
    }

    private void NavigateToBoard(Guid boardId)
    {
        NavigationManager.NavigateTo($"/boards/{boardId}");
    }
}