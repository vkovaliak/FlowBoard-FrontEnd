using FlowBoard.Frontend.Domain.DTOs.Cards;
using FlowBoard.Frontend.Domain.DTOs.Lists;
using FlowBoard.Frontend.Domain.Models.Cards;
using FlowBoard.Frontend.WebApp.Components.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.BoardDetails;

public partial class TaskList
{
    [Inject] public IDialogService DialogService { get; set; } = default!;
    
    [Parameter]
    public ListDto List { get; set; } = default!;
    [Parameter] 
    public Guid BoardId { get; set; }

    [Parameter] 
    public EventCallback<(Guid ListId, string NewName)> OnRenameList { get; set; }

    [Parameter] 
    public EventCallback<(Guid ListId, string ListName)> OnDeleteList { get; set; }

    [Parameter] 
    public EventCallback<CreateCardDto> OnCreateCard { get; set; }

    [Parameter] 
    public EventCallback<(Guid ListId, Guid CardId, UpdateCardDto Dto)> 
        OnUpdateCard { get; set; }
        
    [Parameter] 
    public EventCallback<(Guid ListId, Guid CardId, string CardName)> 
        OnDeleteCard { get; set; }

    private bool _isEditingName = false;
    private string _editedName = string.Empty;

    private bool _isCreatingCard = false;
    private CreateCardModel _cardForm = new();

    private void StartRename()
    {
        _editedName = List.Name;
        _isEditingName = true;
    }

    private void CancelRename()
    {
        _isEditingName = false;
    }

    private async Task SaveRenameAsync()
    {
        if (!string.IsNullOrWhiteSpace(_editedName) && _editedName != List.Name)
        {
            await OnRenameList.InvokeAsync((List.Id, _editedName));
        }
        _isEditingName = false;
    }

    private async Task DeleteListClickAsync()
    {
        await OnDeleteList.InvokeAsync((List.Id, List.Name));
    }

    private void ToggleCreateCardForm()
    {
        _isCreatingCard = !_isCreatingCard;
        _cardForm = new CreateCardModel();
    }

    private async Task SubmitCreateCardAsync()
    {
        if (string.IsNullOrWhiteSpace(_cardForm.Name))
        {
            return;
        } 

        var dto = new CreateCardDto(
            ListId: List.Id,
            BoardId: BoardId,
            Name: _cardForm.Name,
            Description: _cardForm.Description
        );

        await OnCreateCard.InvokeAsync(dto);
        ToggleCreateCardForm();
    }

    private async Task OpenCardDetailsAsync(CardDto card) 
    {
        var parameters = new DialogParameters<EditCardDialog>
        {
            { x => x.CurrentName, card.Name },
            { x => x.CurrentDescription, card.Description }
        };

        var options = new DialogOptions { 
            CloseOnEscapeKey = true, 
            MaxWidth = MaxWidth.Small, 
            FullWidth = true 
        };
        var dialog = await DialogService.ShowAsync<EditCardDialog>(
            "Edit Card", parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled && result.Data is UpdateCardDto updateDto)
        {
            await OnUpdateCard.InvokeAsync((List.Id, card.Id, updateDto));
        }
    }

    private async Task DeleteCardClickAsync(CardDto card)
    {
        await OnDeleteCard.InvokeAsync((List.Id, card.Id, card.Name));
    }
}