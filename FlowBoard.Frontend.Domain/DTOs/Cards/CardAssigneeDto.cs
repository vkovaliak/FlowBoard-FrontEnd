namespace FlowBoard.Frontend.Domain.DTOs.Cards;

public record CardAssigneeDto(
    Guid UserId,
    string EmailAddress);