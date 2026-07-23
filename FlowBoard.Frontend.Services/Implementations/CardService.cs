using FlowBoard.Frontend.Domain.DTOs.Activities;
using FlowBoard.Frontend.Domain.DTOs.Cards;
using FlowBoard.Frontend.Domain.Models.Common;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Helpers;
using FlowBoard.Frontend.Services.Http;

namespace FlowBoard.Frontend.Services.Implementations;

public class CardService : ICardService
{
    private readonly ICardApi _cardApi;

    public CardService(ICardApi cardApi)
    {
        _cardApi = cardApi;
    }

    public async Task<OperationResult> CreateAsync(Guid boardId, CreateCardDto list)
    {
        var response = await _cardApi.CreateAsync(boardId, list);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> UpdateAsync(Guid boardId, Guid listId, Guid cardId, UpdateCardDto card)
    {
        var response = await _cardApi.UpdateAsync(boardId, listId, cardId, card);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> DeleteAsync(Guid boardId, Guid listId, Guid cardId)
    {
        var response = await _cardApi.DeleteAsync(boardId, listId, cardId);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> MoveAsync(Guid boardId, Guid cardId, MoveCardDto card)
    {
        var response = await _cardApi.MoveAsync(boardId, cardId, card);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> AssignMemberAsync(Guid boardId, Guid cardId, Guid userId)
    {
        var response = await _cardApi.AssignMemberAsync(boardId, cardId, userId);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> UnassignMemberAsync(Guid boardId, Guid cardId, Guid userId)
    {
        var response = await _cardApi.UnassignMemberAsync(boardId, cardId, userId);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> ToggleCompletionAsync(Guid boardId, Guid cardId)
    {
        var response = await _cardApi.ToggleCompletionAsync(boardId, cardId);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> RenameAsync(
        Guid boardId, Guid cardId, RenameCardDto dto)
    {
        var response = await _cardApi.RenameAsync(
            boardId, cardId, dto);
            
        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> UpdateDescriptionAsync(
        Guid boardId, Guid cardId, UpdateCardDescriptionDto dto)
    {
        var response = await _cardApi.UpdateDescriptionAsync(
            boardId, cardId, dto);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> SetDueDateAsync(
        Guid boardId, Guid cardId, SetCardDueDateDto dto)
    {
        var response = await _cardApi.SetDueDateAsync(
            boardId, cardId, dto);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<List<MyCardDto>> GetMyTasksAsync()
    {
        var response = await _cardApi.GetMyTasksAsync();

        return response.IsSuccessStatusCode && response.Content != null
            ? response.Content
            : [];
    }

    public async Task<OperationResult<Guid>> DuplicateAsync(
        Guid boardId, Guid cardId)
    {
        var response = await _cardApi.DuplicateAsync(boardId, cardId);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult<Guid>.Ok(response.Content);
        }

        return OperationResult<Guid>.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> SetStartTimeAsync(
        Guid boardId, Guid cardId, SetCardStartTimeDto dto)
    {
        var response = await _cardApi.SetStartTimeAsync(
            boardId, cardId, dto);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<List<ActivityDto>> GetCardActivitiesAsync(Guid boardId, Guid cardId)
    {
        var response = await _cardApi.GetCardActivitiesAsync(boardId, cardId);

        return response.IsSuccessStatusCode && response.Content != null
            ? response.Content
            : [];
    }
}