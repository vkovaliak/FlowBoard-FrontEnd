using FlowBoard.Frontend.WebApp;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Web;
using FlowBoard.Frontend.Services.Extensions;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiUrl = builder.Configuration["ApiUrl"] ?? throw new Exception("ApiUrl missing");;

builder.Services.AddFrontendServices(apiUrl);

await builder.Build().RunAsync();