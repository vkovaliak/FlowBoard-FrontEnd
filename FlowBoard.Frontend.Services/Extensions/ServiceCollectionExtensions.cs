using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Implementations;
using FlowBoard.Frontend.Services.Http;
using FlowBoard.Frontend.Services.Handlers;
using Microsoft.Extensions.Options;
using FlowBoard.Frontend.Services.Configurations;
using Microsoft.Extensions.Configuration;
using FlowBoard.Frontend.Services.Providers;
using Microsoft.AspNetCore.Components.Authorization;

namespace FlowBoard.Frontend.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFrontendServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IBoardService, BoardService>();
        services.AddScoped<CustomAuthStateProvider>();
        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped<AuthenticationStateProvider>(provider => 
            provider.GetRequiredService<CustomAuthStateProvider>());
        
        services.AddTransient<AuthHeaderHandler>();

        services.AddBlazoredLocalStorage();
        services.AddAuthorizationCore();

        var apiOptions = configuration
            .GetSection(ApiOptions.SectionName)
            .Get<ApiOptions>() ?? throw new Exception("ApiOptions missing in configuration");

        services.AddRefitClient<IAuthApi>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri(apiOptions.BaseUrl);
            });
        
        services.AddRefitClient<IBoardApi>()
            .AddHttpMessageHandler<AuthHeaderHandler>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri (apiOptions.BaseUrl);
            });

        return services;
    }
}