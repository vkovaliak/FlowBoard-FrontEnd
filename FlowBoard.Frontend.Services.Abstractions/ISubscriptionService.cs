using FlowBoard.Frontend.Domain.Models.Common;

namespace FlowBoard.Frontend.Services.Abstractions;

public interface ISubscriptionService
{
    Task<string?> CreateCheckoutAsync();
    Task<OperationResult> CancelSubscriptionAsync();
}