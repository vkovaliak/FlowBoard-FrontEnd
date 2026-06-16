using MudBlazor;
using FlowBoard.Frontend.Domain.DTOs.Lists;

namespace FlowBoard.Frontend.WebApp.Pages.BoardDetails;

public partial class BoardDetails
{
    private async Task CreateListAsync(string listName)
    {
        if (string.IsNullOrWhiteSpace(listName))
        {
            Snackbar.Add("List name cannot be empty", Severity.Warning);
            return;
        }

        var newList = new CreateListDto(BoardId: Id, Name: listName);
        var success = await ListService.CreateAsync(newList);

        ShowResult(success,
            "List created successfully!",
            "Failed to create list");
    }

    private async Task HandleRenameListAsync((Guid ListId, string NewName) args)
    {
        if (string.IsNullOrWhiteSpace(args.NewName))
        {
            Snackbar.Add("List name cannot be empty", Severity.Warning);
            return;
        }

        var updateDto = new UpdateListDto(Name: args.NewName);
        var success = await ListService.UpdateAsync(Id, args.ListId, updateDto);

        ShowResult(success,
            "List renamed successfully!",
            "Failed to rename list");
    }

    private async Task HandleDeleteListAsync((Guid ListId, string ListName) args)
    {
        var confirmed = await ConfirmDeleteAsync(
            "Delete List",
            $"Are you sure you want to delete list '{args.ListName}' and all of its cards?");

        if (!confirmed)
            return;

        var success = await ListService.DeleteAsync(Id, args.ListId);

        ShowResult(success,
            $"List '{args.ListName}' deleted",
            "Failed to delete list");
    }
}