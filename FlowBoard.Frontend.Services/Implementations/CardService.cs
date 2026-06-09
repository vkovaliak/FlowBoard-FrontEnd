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

    public async Task<bool> CreateAsync(CreateCardDto list)
    {
        var response = await _cardApi.CreateAsync(list);

        return response.IsSuccessStatusCode 
            && response.Content != Guid.Empty;
    }

    public async Task<bool> UpdateAsync(Guid boardId, Guid listId, Guid cardId, UpdateCardDto list)
    {
        var response = await _cardApi.UpdateAsync(boardId, listId, cardId, list);

        return response.IsSuccessStatusCode 
            && response.Content != false;
    }

    public async Task<bool> DeleteAsync(Guid boardId, Guid listId, Guid cardId)
    {
        var response = await _cardApi.DeleteAsync(boardId, listId, cardId);

        return response.IsSuccessStatusCode 
            && response.Content != false;
    }
}