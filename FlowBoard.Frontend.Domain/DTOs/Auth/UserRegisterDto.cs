namespace FlowBoard.Frontend.Domain.DTOs.Auth;

public record UserRegisterDto(
    string Email, 
    string Password);