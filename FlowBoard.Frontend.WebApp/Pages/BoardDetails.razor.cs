using Microsoft.AspNetCore.Components;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Domain.DTOs.Boards;
using MudBlazor;
using FlowBoard.Frontend.Domain.DTOs.Lists;

namespace FlowBoard.Frontend.WebApp.Pages;

public partial class BoardDetails
{
    [Parameter] 
    public Guid Id { get; set; }

    [Inject] 
    public IBoardService BoardService { get; set; } = default!;
    [Inject]
    public IListService ListService { get; set; } = default!;
    [Inject] 
    public ISnackbar Snackbar { get; set; } = default!;
    [Inject]
    public IDialogService DialogService { get; set; } = default!;

    private BoardDetailsDto? _board;
    private bool _isNotFound = false;

    private bool _isCreatingList = false;
    private string _newListName = string.Empty;

    private void ToggleCreateListForm()
    {
        _isCreatingList = !_isCreatingList;
        _newListName = string.Empty;
    }

    protected override async Task OnInitializedAsync()
    {
        _board = await BoardService.GetDetailsAsync(Id);

        if (_board is null)
        {
            _isNotFound = true;
            Snackbar.Add("Board not found or access denied.", Severity.Error);
        }
    }

    private async Task CreateListAsync(Guid boardId)
    {
        if (string.IsNullOrWhiteSpace(_newListName))
        {
            Snackbar.Add("List name cannot be empty", Severity.Warning);
            return;
        }
        
        var newList = new CreateListDto(
            BoardId: boardId,
            Name: _newListName);

        var success = await ListService.CreateAsync(newList);

        if (success)
        {
            Snackbar.Add("List created successfully!", Severity.Success);
            
            _board = await BoardService.GetDetailsAsync(Id); 
            
            ToggleCreateListForm();
        }
        else
        {
            Snackbar.Add("Failed to create list", Severity.Error);
        }
    }

    private async Task HandleRenameListAsync(
        (Guid ListId, string NewName) args)
    {
        if (string.IsNullOrWhiteSpace(args.NewName))
        {
            Snackbar.Add("List name cannot be empty", Severity.Warning);
            return;
        }

        var updateDto = new UpdateListDto(Name: args.NewName);

        var success = await ListService.UpdateAsync(Id, args.ListId, updateDto);

        if (success)
        {
            Snackbar.Add("List renamed successfully!", Severity.Success); 
            _board = await BoardService.GetDetailsAsync(Id);
        }
        else
        {
            Snackbar.Add("Failed to rename list", Severity.Error);
        }
    }
    
    private async Task HandleDeleteListAsync(
        (Guid ListId, string ListName) args)
    {
        bool? result = await DialogService.ShowMessageBoxAsync(
            "Delete List", 
            $"Are you sure you want to delete list '{args.ListName}' and all of its cards?", 
            yesText: "Delete", 
            cancelText: "Cancel");

        if (result == true)
        {
            var success = await ListService.DeleteAsync(Id, args.ListId);

            if (success)
            {
                Snackbar.Add($"List '{args.ListName}' deleted", Severity.Success);
                _board = await BoardService.GetDetailsAsync(Id);
            }
            else
            {
                Snackbar.Add("Failed to delete list", Severity.Error);
            }
        }
    }
    
}