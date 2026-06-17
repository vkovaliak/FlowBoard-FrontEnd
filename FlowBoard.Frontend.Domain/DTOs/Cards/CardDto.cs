using FlowBoard.Frontend.Domain.DTOs.Attachments;

namespace FlowBoard.Frontend.Domain.DTOs.Cards;

public record CardDto(
    Guid Id,
    Guid ListId,
    string Name,
    string? Description,
    int Position,
    DateTime? DueDate,
    bool IsCompleted,
    List<AttachmentResponseDto> Attachments,
    List<CardAssigneeDto> Assignees
);