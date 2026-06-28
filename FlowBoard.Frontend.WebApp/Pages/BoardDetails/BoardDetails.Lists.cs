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

        var newList = new CreateListDto(Name: listName);
        var result = await ListService.CreateAsync(Id, newList);

        ShowResult(result.Success,
            "List created successfully!",
            result.Error ?? "Failed");
    }

    private async Task HandleRenameListAsync((Guid ListId, string NewName) args)
    {
        if (string.IsNullOrWhiteSpace(args.NewName))
        {
            Snackbar.Add("List name cannot be empty", Severity.Warning);
            return;
        }

        var updateDto = new UpdateListDto(Name: args.NewName);
        var result = await ListService.UpdateAsync(Id, args.ListId, updateDto);

        ShowResult(result.Success,
            "List renamed successfully!",
            result.Error ?? "Failed");
    }

    private async Task HandleDeleteListAsync((Guid ListId, string ListName) args)
    {
        var confirmed = await ConfirmDeleteAsync(
            "Delete List",
            $"Are you sure you want to delete list '{args.ListName}' and all of its cards?");

        if (!confirmed)
            return;

        var result = await ListService.DeleteAsync(Id, args.ListId);

        ShowResult(result.Success,
            $"List '{args.ListName}' deleted",
            result.Error ?? "Failed");
    }
}