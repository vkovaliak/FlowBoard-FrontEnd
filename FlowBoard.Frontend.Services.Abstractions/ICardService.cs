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
    Task<bool> ToggleCompletionAsync(Guid boardId, Guid cardId);
    Task<bool> RenameAsync(Guid boardId, Guid cardId, RenameCardDto dto);
    Task<bool> UpdateDescriptionAsync(
        Guid boardId, Guid cardId, UpdateCardDescriptionDto dto);
    Task<bool> SetDueDateAsync(Guid boardId, Guid cardId, SetCardDueDateDto dto);

    Task<List<MyCardDto>> GetMyTasksAsync();
}