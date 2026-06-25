using FlowBoard.Frontend.Domain.DTOs.Cards;
using Refit;

namespace FlowBoard.Frontend.Services.Http;

public interface ICardApi
{
    [Post("/api/boards/{boardId}/cards")]
    Task<ApiResponse<Guid>> CreateAsync(Guid boardId, [Body] CreateCardDto dto);

    [Put("/api/boards/{boardId}/lists/{listId}/cards/{cardId}")]
    Task<ApiResponse<bool>> UpdateAsync(Guid boardId, Guid listId, Guid cardId, [Body] UpdateCardDto dto);

    [Delete("/api/boards/{boardId}/lists/{listId}/cards/{cardId}")]
    Task<ApiResponse<bool>> DeleteAsync(Guid boardId, Guid listId, Guid cardId);

    [Put("/api/boards/{boardId}/cards/{cardId}/move")]
    Task<ApiResponse<bool>> MoveAsync(Guid boardId, Guid cardId, [Body] MoveCardDto dto);

    [Post("/api/boards/{boardId}/cards/{cardId}/assignees/{userId}")]
    Task<ApiResponse<bool>> AssignMemberAsync(Guid boardId, Guid cardId, Guid userId);

    [Delete("/api/boards/{boardId}/cards/{cardId}/assignees/{userId}")]
    Task<ApiResponse<bool>> UnassignMemberAsync(Guid boardId, Guid cardId, Guid userId);
    
    [Put("/api/boards/{boardId}/cards/{cardId}/toggle-completion")]
    Task<ApiResponse<bool>> ToggleCompletionAsync(Guid boardId, Guid cardId);

    [Put("/api/boards/{boardId}/cards/{cardId}/name")]
    Task<ApiResponse<bool>> RenameAsync(
        Guid boardId, Guid cardId, [Body] RenameCardDto dto);

    [Put("/api/boards/{boardId}/cards/{cardId}/description")]
    Task<ApiResponse<bool>> UpdateDescriptionAsync(
        Guid boardId, Guid cardId, [Body] UpdateCardDescriptionDto dto);

    [Put("/api/boards/{boardId}/cards/{cardId}/due-date")]
    Task<ApiResponse<bool>> SetDueDateAsync(
        Guid boardId, Guid cardId, [Body] SetCardDueDateDto dto);

    [Get("/api/my-tasks")]
    Task<ApiResponse<List<MyCardDto>>> GetMyTasksAsync();
}