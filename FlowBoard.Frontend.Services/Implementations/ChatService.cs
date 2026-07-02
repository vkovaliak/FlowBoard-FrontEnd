using FlowBoard.Frontend.Domain.DTOs.Chat;
using FlowBoard.Frontend.Domain.Models.Common;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Helpers;
using FlowBoard.Frontend.Services.Http;

namespace FlowBoard.Frontend.Services.Implementations;

public class ChatService : IChatService
{
    private readonly IChatApi _chatApi;

    public ChatService(IChatApi chatApi)
    {
        _chatApi = chatApi;
    }

    public async Task<OperationResult<ChatResponse?>> SendMessageAsync(
        ChatRequest request)
    {
        var response = await _chatApi.SendMessageAsync(request);

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            return OperationResult<ChatResponse?>.Ok(response.Content);
        }

        return OperationResult<ChatResponse?>.Fail(response.GetErrorMessage());
    }
}