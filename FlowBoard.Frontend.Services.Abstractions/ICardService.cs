using FlowBoard.Frontend.Domain.DTOs.Activities;
using FlowBoard.Frontend.Domain.DTOs.Cards;
using FlowBoard.Frontend.Domain.Models.Common;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface ICardService
{
    Task<OperationResult> CreateAsync(Guid boardId, CreateCardDto dto);
    Task<OperationResult> UpdateAsync(Guid boardId, Guid listId, Guid cardId, UpdateCardDto dto);
    Task<OperationResult> DeleteAsync(Guid boardId, Guid listId, Guid cardId);
    Task<OperationResult> MoveAsync(Guid boardId, Guid cardId, MoveCardDto dto);
    Task<OperationResult> AssignMemberAsync(Guid boardId, Guid cardId, Guid userId);
    Task<OperationResult> UnassignMemberAsync(Guid boardId, Guid cardId, Guid userId);
    Task<OperationResult> ToggleCompletionAsync(Guid boardId, Guid cardId);
    Task<OperationResult> RenameAsync(Guid boardId, Guid cardId, RenameCardDto dto);
    Task<OperationResult> UpdateDescriptionAsync(
        Guid boardId, Guid cardId, UpdateCardDescriptionDto dto);
    Task<OperationResult> SetDueDateAsync(Guid boardId, Guid cardId, SetCardDueDateDto dto);
    Task<List<MyCardDto>> GetMyTasksAsync();
    Task<OperationResult<Guid>> DuplicateAsync(Guid boardId, Guid cardId);
    Task<OperationResult> SetStartTimeAsync(Guid boardId, Guid cardId, SetCardStartTimeDto dto);
    Task<List<ActivityDto>> GetCardActivitiesAsync(Guid boardId, Guid cardId);
}