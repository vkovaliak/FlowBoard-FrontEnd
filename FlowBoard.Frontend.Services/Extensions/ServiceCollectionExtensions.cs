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

namespace FlowBoard.Frontend.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFrontendServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();

        services.AddTransient<AuthHeaderHandler>();

        services.AddBlazoredLocalStorage();

        var apiOptions = configuration
            .GetSection(ApiOptions.SectionName)
            .Get<ApiOptions>() ?? throw new Exception("ApiOptions missing in configuration");

        services.AddRefitClient<IAuthApi>()
            //.AddHttpMessageHandler<AuthHeaderHandler>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri(apiOptions.BaseUrl);
            });

        return services;
    }
}