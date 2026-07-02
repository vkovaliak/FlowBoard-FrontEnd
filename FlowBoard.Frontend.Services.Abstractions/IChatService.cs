using FlowBoard.Frontend.Domain.DTOs.Chat;
using FlowBoard.Frontend.Domain.Models.Common;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface IChatService
{
    Task<OperationResult<ChatResponse?>> SendMessageAsync(
        ChatRequest request);
}