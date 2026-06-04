using FlowBoard.Frontend.WebApp;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Web;
using FlowBoard.Frontend.Services.Extensions;
using FlowBoard.Frontend.Services.Configurations;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.Configure<ApiOptions>(
    builder.Configuration.GetSection(ApiOptions.SectionName));

builder.Services.AddFrontendServices();

await builder.Build().RunAsync();