namespace FlowBoard.Frontend.Domain.DTOs.Lists;

public record CreateListDto(
    Guid BoardId,
    string Name);