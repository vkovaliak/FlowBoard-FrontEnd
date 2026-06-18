namespace FlowBoard.Frontend.Domain.DTOs.Checklists;

public record ChecklistItemDto(
    Guid Id,
    Guid CardId,
    string Text,
    bool IsCompleted,
    int Position);