namespace FlowBoard.Frontend.Domain.DTOs.Chat;

public record ChatMessage(
    string Text, bool IsUser);