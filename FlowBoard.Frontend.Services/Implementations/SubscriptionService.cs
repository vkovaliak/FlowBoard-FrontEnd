using FlowBoard.Frontend.Domain.Models.Common;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Helpers;
using FlowBoard.Frontend.Services.Http;

namespace FlowBoard.Frontend.Services.Implementations;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionApi _subscriptionApi;

    public SubscriptionService(ISubscriptionApi subscriptionApi)
    {
        _subscriptionApi = subscriptionApi;
    }

    public async Task<string?> CreateCheckoutAsync()
    {
        var response = await _subscriptionApi.CreateCheckoutAsync();

        return response.IsSuccessStatusCode
            ? response.Content
            : null;
    }

    public async Task<OperationResult> CancelSubscriptionAsync()
    {
        var response = await _subscriptionApi.CancelSubscriptionAsync();
        
        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }
}