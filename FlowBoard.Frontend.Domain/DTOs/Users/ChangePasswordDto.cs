namespace FlowBoard.Frontend.Domain.DTOs.Users;

public record ChangePasswordDto(
    string CurrentPassword,
    string NewPassword,
    string ConfirmPassword
);