namespace FlowBoard.Frontend.Domain.DTOs.Labels;

public record LabelDto(
    Guid Id,
    string Name,
    string Color
);