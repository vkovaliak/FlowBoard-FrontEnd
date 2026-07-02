using FlowBoard.Frontend.Domain.DTOs.Chat;
using Refit;

namespace FlowBoard.Frontend.Services.Http;

public interface IChatApi
{
    [Post("/api/chat")]
    Task<ApiResponse<ChatResponse>> SendMessageAsync(ChatRequest request);

}