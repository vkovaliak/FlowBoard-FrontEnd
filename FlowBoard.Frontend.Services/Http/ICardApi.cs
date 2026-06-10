using FlowBoard.Frontend.Domain.DTOs.Cards;
using Refit;

namespace FlowBoard.Frontend.Services.Http;

public interface ICardApi
{
    [Post("/api/cards")]
    Task<ApiResponse<Guid>> CreateAsync([Body] CreateCardDto dto);

    [Put("/api/cards/{boardId}/list/{listId}/card/{cardId}")]
    Task<ApiResponse<bool>> UpdateAsync(Guid boardId, Guid listId, Guid cardId, [Body] UpdateCardDto dto);

    [Delete("/api/cards/{boardId}/list/{listId}/card/{cardId}")]
    Task<ApiResponse<bool>> DeleteAsync(Guid boardId, Guid listId, Guid cardId);

    [Put("/api/cards/{boardId}/card/{cardId}/move")]
    Task<ApiResponse<bool>> MoveAsync(Guid boardId, Guid cardId, [Body] MoveCardDto dto);
}