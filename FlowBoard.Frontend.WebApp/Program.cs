using Blazored.LocalStorage;
using Refit;
using FlowBoard.Frontend.WebApp;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Implementations;
using FlowBoard.Frontend.Services.Http;
using FlowBoard.Frontend.Services.Handlers;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Web;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiUrl = "http://localhost:5009";

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddTransient<AuthHeaderHandler>();

builder.Services.AddRefitClient<IAuthApi>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(apiUrl);
    });


await builder.Build().RunAsync();