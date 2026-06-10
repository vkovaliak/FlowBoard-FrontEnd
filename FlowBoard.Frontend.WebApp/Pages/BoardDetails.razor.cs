using Microsoft.AspNetCore.Components;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Domain.DTOs.Boards;
using MudBlazor;
using FlowBoard.Frontend.Domain.DTOs.Lists;
using FlowBoard.Frontend.Domain.DTOs.Cards;
using Microsoft.AspNetCore.Authorization;
using FlowBoard.Frontend.WebApp.Components.Dialogs;

namespace FlowBoard.Frontend.WebApp.Pages;

[Authorize]
public partial class BoardDetails
{
    [Parameter] 
    public Guid Id { get; set; }

    [Inject] 
    public IBoardService BoardService { get; set; } = default!;
    [Inject]
    public ICardService CardService { get; set; } = default!;
    [Inject]
    public IListService ListService { get; set; } = default!;
    [Inject] 
    public ISnackbar Snackbar { get; set; } = default!;
    [Inject]
    public IDialogService DialogService { get; set; } = default!;

    private BoardDetailsDto? _board;
    private bool _isNotFound = false;

    private async Task RefreshBoardAsync()
    {
        _board = await BoardService.GetDetailsAsync(Id);
    }

    protected override async Task OnInitializedAsync()
    {
        await RefreshBoardAsync();

        if (_board is null)
        {
            _isNotFound = true;
            Snackbar.Add("Board not found or access denied.", Severity.Error);
        }
    }

    private async Task CreateListAsync(string listName)
    {
        if (string.IsNullOrWhiteSpace(listName))
        {
            Snackbar.Add("List name cannot be empty", Severity.Warning);
            return;
        }
        
        var newList = new CreateListDto(
            BoardId: Id,
            Name: listName);

        var success = await ListService.CreateAsync(newList);

        if (success)
        {
            Snackbar.Add("List created successfully!", Severity.Success);
            await RefreshBoardAsync();
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
            await RefreshBoardAsync();
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
                await RefreshBoardAsync();
            }
            else
            {
                Snackbar.Add("Failed to delete list", Severity.Error);
            }
        }
    }

    private async Task HandleCreateCardAsync(CreateCardDto dto)
    {
        var success = await CardService.CreateAsync(dto);

        if (success)
        {
            Snackbar.Add("Card added!", Severity.Success);
            await RefreshBoardAsync();
        }
        else
        {
            Snackbar.Add("Failed to add card", Severity.Error);
        }
    }

    private async Task HandleUpdateCardAsync(
        (Guid ListId, Guid CardId, UpdateCardDto Dto) args)
    {
        var success = await CardService.UpdateAsync(
            Id, args.ListId, args.CardId, args.Dto);

        if (success)
        {
            Snackbar.Add("Card updated successfully!", Severity.Success);
            await RefreshBoardAsync();
        }
        else
        {
            Snackbar.Add("Failed to update card", Severity.Error);
        }
    }

    private async Task HandleDeleteCardAsync(
        (Guid ListId, Guid CardId, string CardName) args)
    {
        bool? result = await DialogService.ShowMessageBoxAsync(
            "Delete Card", 
            $"Are you sure you want to delete card '{args.CardName}'?", 
            yesText: "Delete", 
            cancelText: "Cancel");

        if (result == true)
        {
            var success = await CardService.DeleteAsync(
                Id, args.ListId, args.CardId);

            if (success)
            {
                Snackbar.Add($"Card '{args.CardName}' deleted", Severity.Success);
                _board = await BoardService.GetDetailsAsync(Id);
            }
            else
            {
                Snackbar.Add("Failed to delete card", Severity.Error);
            }
        }
    }

    private async Task OpenInviteDialogAsync()
    {
        var options = new DialogOptions 
        { 
            CloseOnEscapeKey = true, 
            MaxWidth = MaxWidth.Small, 
            FullWidth = true 
        };

        var dialog = await DialogService.ShowAsync<
            InviteMemberDialog>("Invite Member", options);
        var result = await dialog.Result;

        if (!result!.Canceled && result.Data is string email)
        {
            await HandleInviteMemberAsync(email);
        }
    }    

    private async Task HandleInviteMemberAsync(string email)
    {
        var inviteDto = new InviteMemberDto(Email: email);
        var result = await BoardService.InviteMemberAsync(Id, inviteDto);

        if (result)
        {
            Snackbar.Add($"User {email} successfully invited!", Severity.Success);
            await RefreshBoardAsync();
        }
        else
        {
            Snackbar.Add("Failed to invite user.", Severity.Error);
        }
    }

    private async Task HandleListDropped(MudItemDropInfo<ListDto> drop)
    {
        var dto = new MoveListDto(
            NewPosition: drop.IndexInZone
        );

        var success = await ListService.MoveAsync(
            Id,
            drop.Item!.Id,
            dto);

        if (!success)
        {
            Snackbar.Add("Failed to move list", Severity.Error);

            await RefreshBoardAsync();
        }
    }
}