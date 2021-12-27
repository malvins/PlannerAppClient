using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using Blazored.LocalStorage;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Authorization;

namespace PlannerAppClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddHttpClient("PlannerApp.Api", client => {
                client.BaseAddress = new Uri("https://plannerapp-api.azurewebsites.net");
            }).AddHttpMessageHandler<AuthorizationMessageHandler>();
            builder.Services.AddTransient<AuthorizationMessageHandler>();
            builder.Services.AddScoped(sp => sp.GetService<IHttpClientFactory>().CreateClient("PlannerApp.Api"));
            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddMudServices();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();

            await builder.Build().RunAsync();
        }

    }
}
