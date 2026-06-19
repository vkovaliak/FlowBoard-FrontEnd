using FlowBoard.Frontend.Domain.DTOs.Cards;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Http;

namespace FlowBoard.Frontend.Services.Implementations;

public class CardService : ICardService
{
    private readonly ICardApi _cardApi;

    public CardService(ICardApi cardApi)
    {
        _cardApi = cardApi;
    }

    public async Task<bool> CreateAsync(Guid boardId, CreateCardDto list)
    {
        var response = await _cardApi.CreateAsync(boardId, list);

        return response.IsSuccessStatusCode 
            && response.Content != Guid.Empty;
    }

    public async Task<bool> UpdateAsync(Guid boardId, Guid listId, Guid cardId, UpdateCardDto card)
    {
        var response = await _cardApi.UpdateAsync(boardId, listId, cardId, card);

        return response.IsSuccessStatusCode 
            && response.Content != false;
    }

    public async Task<bool> DeleteAsync(Guid boardId, Guid listId, Guid cardId)
    {
        var response = await _cardApi.DeleteAsync(boardId, listId, cardId);

        return response.IsSuccessStatusCode 
            && response.Content != false;
    }

    public async Task<bool> MoveAsync(Guid boardId, Guid cardId, MoveCardDto card)
    {
        var response = await _cardApi.MoveAsync(boardId, cardId, card);

        return response.IsSuccessStatusCode 
            && response.Content != false;
    }

    public async Task<bool> AssignMemberAsync(Guid boardId, Guid cardId, Guid userId)
    {
        var response = await _cardApi.AssignMemberAsync(boardId, cardId, userId);

        return response.IsSuccessStatusCode
            && response.Content != false;
    }

    public async Task<bool> UnassignMemberAsync(Guid boardId, Guid cardId, Guid userId)
    {
        var response = await _cardApi.UnassignMemberAsync(boardId, cardId, userId);

        return response.IsSuccessStatusCode
            && response.Content != false;
    }

    public async Task<bool> ToggleCompletionAsync(Guid boardId, Guid cardId)
    {
        var response = await _cardApi.ToggleCompletionAsync(boardId, cardId);

        return response.IsSuccessStatusCode
            && response.Content != false;
    }

    public async Task<bool> RenameAsync(
        Guid boardId, Guid cardId, RenameCardDto dto)
    {
        var response = await _cardApi.RenameAsync(
            boardId, cardId, dto);
            
        return response.IsSuccessStatusCode 
            && response.Content != false;
    }

    public async Task<bool> UpdateDescriptionAsync(
        Guid boardId, Guid cardId, UpdateCardDescriptionDto dto)
    {
        var response = await _cardApi.UpdateDescriptionAsync(
            boardId, cardId, dto);

        return response.IsSuccessStatusCode 
            && response.Content != false;
    }

    public async Task<bool> SetDueDateAsync(
        Guid boardId, Guid cardId, SetCardDueDateDto dto)
    {
        var response = await _cardApi.SetDueDateAsync(
            boardId, cardId, dto);

        return response.IsSuccessStatusCode 
            && response.Content != false;
    }
}