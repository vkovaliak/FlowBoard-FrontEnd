using FlowBoard.Frontend.Domain.Constants;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace FlowBoard.Frontend.Services.Implementations;

public class MicrosoftAuthService : IMicrosoftAuthService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly EntraIdOptions _entraIdOptions;
    private bool _isInitialized;

    public MicrosoftAuthService(
        IJSRuntime jsRuntime, 
        IOptions<EntraIdOptions> entraIdOptions)
    {
        _jsRuntime = jsRuntime;
        _entraIdOptions = entraIdOptions.Value;
    }
    public async Task<string?> GetIdTokenAsync()
    {
        if (!_isInitialized)
        {
            await _jsRuntime.InvokeVoidAsync(
                AuthConstants.JsMethods.Initialize, 
                _entraIdOptions.ClientId, 
                _entraIdOptions.Authority,
                _entraIdOptions.RedirectUri);
            _isInitialized = true;
        }

        return await _jsRuntime.InvokeAsync<string?>(
            AuthConstants.JsMethods.LoginPopup);
    }
}