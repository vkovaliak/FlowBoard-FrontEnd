namespace FlowBoard.Frontend.Services.Abstractions;

public interface ISubscriptionService
{
    Task<string?> CreateCheckoutAsync();
}