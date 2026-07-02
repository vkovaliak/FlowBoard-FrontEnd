using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Implementations;
using FlowBoard.Frontend.Services.Http;
using FlowBoard.Frontend.Services.Handlers;
using FlowBoard.Frontend.Services.Configurations;
using Microsoft.Extensions.Configuration;
using FlowBoard.Frontend.Services.Providers;
using Microsoft.AspNetCore.Components.Authorization;
using FlowBoard.Frontend.Services.State;

namespace FlowBoard.Frontend.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFrontendServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IAttachmentService, AttachmentService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IBoardService, BoardService>();
        services.AddScoped<IBoardHubService, BoardHubService>();
        services.AddScoped<ICardService, CardService>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<IChecklistService, ChecklistService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<ICommentHubService, CommentHubService>();
        services.AddScoped<ILabelService, LabelService>();
        services.AddScoped<IListService, ListService>();
        services.AddScoped<IMicrosoftAuthService, MicrosoftAuthService>();
        services.AddScoped<CustomAuthStateProvider>();
        services.AddScoped<ISearchService, SearchService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<FavoritesState>();
        services.AddScoped<UserState>();

        services.AddScoped<AuthenticationStateProvider>(provider => 
            provider.GetRequiredService<CustomAuthStateProvider>());
        
        services.AddTransient<AuthHeaderHandler>();

        services.AddBlazoredLocalStorage();
        services.AddAuthorizationCore();

        services.Configure<ApiOptions>(
            configuration.GetSection(ApiOptions.SectionName));

        var apiOptions = configuration
            .GetSection(ApiOptions.SectionName)
            .Get<ApiOptions>() ?? throw new Exception(
                "ApiOptions missing in configuration");

        services.AddRefitClient<IAuthApi>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri(apiOptions.BaseUrl);
            });

        void AddAuthenticatedRefitClient<T>() where T : class
        {
            services.AddRefitClient<T>()
                .AddHttpMessageHandler<AuthHeaderHandler>()
                .ConfigureHttpClient(client =>
                {
                    client.BaseAddress = new Uri (apiOptions.BaseUrl);
                });
        }

        services.Configure<EntraIdOptions>(
            configuration.GetSection(EntraIdOptions.SectionName));
        
        AddAuthenticatedRefitClient<IAttachmentApi>();
        AddAuthenticatedRefitClient<IBoardApi>();
        AddAuthenticatedRefitClient<ICardApi>();
        AddAuthenticatedRefitClient<IChatApi>();
        AddAuthenticatedRefitClient<IChecklistApi>();
        AddAuthenticatedRefitClient<ICommentApi>();
        AddAuthenticatedRefitClient<IListApi>();
        AddAuthenticatedRefitClient<ILabelApi>();
        AddAuthenticatedRefitClient<ISearchApi>();
        AddAuthenticatedRefitClient<IUserApi>();
        
        return services;
    }
}