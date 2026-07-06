using Refit;

namespace FlowBoard.Frontend.Services.Http;

public interface ISubscriptionApi
{
    [Post("/api/subscription/checkout")]
    Task<ApiResponse<string>> CreateCheckoutAsync();
}