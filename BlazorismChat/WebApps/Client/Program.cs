using BlazorismChat.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using BlazorismChat.ClientLibraries.Security;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System;
using BlazorismChat.ClientLibraries.ClientServices.Interfaces;
using BlazorismChat.ClientLibraries.ClientServices;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<AuthenticationStateProvider, BlazorismChatAuthenticationState>();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddTransient<IUserService, UserService>();

builder.RootComponents.Add<HeadOutlet>("head::after");

await builder.Build().RunAsync();
