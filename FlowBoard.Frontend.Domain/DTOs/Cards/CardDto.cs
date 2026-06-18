using FlowBoard.Frontend.Domain.DTOs.Attachments;
using FlowBoard.Frontend.Domain.DTOs.Labels;

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
    List<CardAssigneeDto> Assignees,
    List<LabelDto> Labels
);