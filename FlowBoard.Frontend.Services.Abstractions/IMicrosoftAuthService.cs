namespace FlowBoard.Frontend.Services.Abstractions;

public interface IMicrosoftAuthService
{
    Task<string?> GetIdTokenAsync();
}