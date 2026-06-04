using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Implementations;
using FlowBoard.Frontend.Services.Http;
using FlowBoard.Frontend.Services.Handlers;
using Microsoft.Extensions.Options;
using FlowBoard.Frontend.Services.Configurations;

namespace FlowBoard.Frontend.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFrontendServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();

        services.AddTransient<AuthHeaderHandler>();

        services.AddBlazoredLocalStorage();

        services.AddRefitClient<IAuthApi>()
            .ConfigureHttpClient((sp, c)=>
            {
                var options = sp.GetRequiredService<IOptions<ApiOptions>>();
                c.BaseAddress = new Uri(options.Value.BaseUrl);
            });

        return services;
    }
}