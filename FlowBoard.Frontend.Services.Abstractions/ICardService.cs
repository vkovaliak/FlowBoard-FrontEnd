using FlowBoard.Frontend.Domain.DTOs.Cards;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface ICardService
{
    Task<bool> CreateAsync(Guid boardId, CreateCardDto dto);
    Task<bool> UpdateAsync(Guid boardId, Guid listId, Guid cardId, UpdateCardDto dto);
    Task<bool> DeleteAsync(Guid boardId, Guid listId, Guid cardId);
    Task<bool> MoveAsync(Guid boardId, Guid cardId, MoveCardDto dto);
    Task<bool> AssignMemberAsync(Guid boardId, Guid cardId, Guid userId);
    Task<bool> UnassignMemberAsync(Guid boardId, Guid cardId, Guid userId);
}