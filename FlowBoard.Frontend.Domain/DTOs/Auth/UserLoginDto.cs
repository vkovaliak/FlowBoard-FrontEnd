namespace FlowBoard.Frontend.Domain.DTOs.Auth;

public record UserLoginDto(
    string Email,
    string Password);