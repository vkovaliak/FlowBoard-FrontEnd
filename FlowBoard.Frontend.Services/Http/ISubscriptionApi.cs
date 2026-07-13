using Refit;

namespace FlowBoard.Frontend.Services.Http;

public interface ISubscriptionApi
{
    [Post("/api/subscription/checkout")]
    Task<ApiResponse<string>> CreateCheckoutAsync();

    [Post("/api/subscription/cancel")]
    Task<ApiResponse<bool>> CancelSubscriptionAsync();
}