using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components.Authorization;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Domain.Constants;

namespace FlowBoard.Frontend.Services.Providers;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ITokenService _tokenService;
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    public CustomAuthStateProvider(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await _tokenService.GetAccessTokenAsync();
            if (string.IsNullOrWhiteSpace(token))
            {
                return new AuthenticationState(_anonymous);
            }

            var expiration = await _tokenService.GetAccessTokenExpirationAsync();
            if (expiration is null || expiration.Value <= DateTime.UtcNow)
            {
                return new AuthenticationState(_anonymous);
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var identity = new ClaimsIdentity(jwtToken.Claims, AuthConstants.SchemeName);
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }
        catch
        {
            return new AuthenticationState(_anonymous);
        }
    }


    public async Task<Guid> GetCurrentUserIdAsync()
    {
        var authState = await GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity is { IsAuthenticated: true })
        {
            var userIdStr = user.FindFirst(c => c.Type == "sub")?.Value 
                            ?? user.FindFirst(
                                c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userIdStr, out var parsedId))
            {
                return parsedId;
            }
        }

        return Guid.Empty;
    }

    public void NotifyUserAuthentication(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var identity = new ClaimsIdentity(jwtToken.Claims, AuthConstants.SchemeName);
        var user = new ClaimsPrincipal(identity);
        var state = Task.FromResult(new AuthenticationState(user));
        
        NotifyAuthenticationStateChanged(state);
    }

    public void NotifyUserLogout()
    {
        var state = Task.FromResult(new AuthenticationState(_anonymous));
        NotifyAuthenticationStateChanged(state);
    }
}