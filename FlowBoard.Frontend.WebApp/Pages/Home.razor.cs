using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authorization;
using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Services.Abstractions;
using MudBlazor;
using FlowBoard.Frontend.WebApp.Components.Dialogs;
using FlowBoard.Frontend.Domain.Models.Boards;

namespace FlowBoard.Frontend.WebApp.Pages;

[Authorize]
public partial class Home
{
    [Inject] public IBoardService BoardService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public ISnackbar Snackbar { get; set; } = default!;

    private IEnumerable<BoardDto>? _boards;

    protected override async Task OnInitializedAsync()
    {
        _boards = await BoardService.GetMyBoardsAsync();
    }

    private async Task OpenCreateBoardDialog()
    {
        var options = new DialogOptions { 
            CloseOnEscapeKey = true, FullWidth = true,
            MaxWidth = MaxWidth.ExtraSmall 
        };
        
        var dialog = await DialogService.ShowAsync<CreateBoardDialog>(
            string.Empty, options);

        var dialogResult = await dialog.Result;

        if (dialogResult is { 
                Canceled: false, 
                Data: CreateBoardModel createdBoardModel 
            })
        {
            var board = new CreateBoardDto(
                Name: createdBoardModel.Name,
                IsPublic: createdBoardModel.IsPublic
            );

            var result = await BoardService.CreateAsync(board);

            if (result)
            {
                Snackbar.Add("Board created!", Severity.Success);
                _boards = await BoardService.GetMyBoardsAsync();
            }
            else
            {
                Snackbar.Add("Board failed to create", Severity.Error);
            }   
        }
    }
}