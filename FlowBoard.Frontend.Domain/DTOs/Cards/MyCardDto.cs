
using FlowBoard.Frontend.Domain.DTOs.Labels;

namespace FlowBoard.Frontend.Domain.DTOs.Cards;

public record MyCardDto(
    Guid Id,
    Guid ListId,
    Guid BoardId,
    string BoardName,
    string Name,
    string? Description,
    DateTime? StartTime,
    DateTime? DueDate,
    bool IsCompleted,
    List<LabelDto> Labels,
    List<CardAssigneeDto> Assignees);