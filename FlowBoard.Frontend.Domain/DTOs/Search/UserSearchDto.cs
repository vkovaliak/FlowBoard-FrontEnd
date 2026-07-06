namespace FlowBoard.Frontend.Domain.DTOs.Search;

public record UserSearchDto(
    Guid Id,
    string EmailAddress,
    string UserName,
    string? AvatarUrl);