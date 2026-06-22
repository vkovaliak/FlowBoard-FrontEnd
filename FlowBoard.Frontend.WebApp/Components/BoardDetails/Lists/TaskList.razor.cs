using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Domain.DTOs.Cards;
using FlowBoard.Frontend.Domain.DTOs.Lists;
using FlowBoard.Frontend.WebApp.Components.BoardDetails.Cards;
using FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.BoardDetails.Lists;

public partial class TaskList
{
    [Inject] public IDialogService DialogService { get; set; } = default!;

    [Parameter] public ListDto List { get; set; } = default!;
    [Parameter] public List<ListDto> AllLists { get; set; } = [];
    [Parameter] public Guid BoardId { get; set; }
    [Parameter] public List<BoardMemberDto> BoardMembers { get; set; } = [];

    [Parameter] public EventCallback<(Guid ListId, string NewName)> OnRenameList { get; set; }
    [Parameter] public EventCallback<(Guid ListId, string ListName)> OnDeleteList { get; set; }
    [Parameter] public EventCallback<CreateCardDto> OnCreateCard { get; set; }
    [Parameter] public EventCallback<(Guid ListId, Guid CardId, UpdateCardDto Dto)> OnUpdateCard { get; set; }
    [Parameter] public EventCallback<(Guid ListId, Guid CardId, string CardName)> OnDeleteCard { get; set; }
    [Parameter] public EventCallback<CardDto> OnToggleCardComplete { get; set; }

    private CreateCardForm _createCardForm = default!;

    private void OpenCreateCardForm() => _createCardForm.Open();

    private async Task RenameListAsync(string newName)
        => await OnRenameList.InvokeAsync((List.Id, newName));

    private async Task DeleteListClickAsync()
        => await OnDeleteList.InvokeAsync((List.Id, List.Name));

    private async Task CreateCardAsync(string cardName)
    {
        var dto = new CreateCardDto(
            ListId: List.Id,
            Name: cardName,
            Description: null);

        await OnCreateCard.InvokeAsync(dto);
    }

    private async Task DeleteCardClickAsync(CardDto card)
        => await OnDeleteCard.InvokeAsync((List.Id, card.Id, card.Name));

    private async Task ToggleCardCompleteAsync(CardDto card)
        => await OnToggleCardComplete.InvokeAsync(card);

    private async Task OpenCardDetailsAsync(CardDto card)
    {
        var parameters = new DialogParameters<EditCardDialog>
        {
            { x => x.BoardId, BoardId },
            { x => x.CardId, card.Id }
        };

        var options = new DialogOptions
        {
            Position = DialogPosition.CenterRight,
            MaxWidth = MaxWidth.False,
            FullWidth = false,
            CloseButton = false,
            NoHeader = false
        };

        var dialog = await DialogService.ShowAsync<EditCardDialog>(
            null, parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled && result.Data is UpdateCardDto updateDto)
        {
            await OnUpdateCard.InvokeAsync((List.Id, card.Id, updateDto));
        }
    }
}