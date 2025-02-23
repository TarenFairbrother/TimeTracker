using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TimeTracker.Client;
using TimeTracker.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ITimeEntryService, TimeEntryService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IAuthservice, AuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddAuthorizationCore();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddBlazoredToast();

await builder.Build().RunAsync();