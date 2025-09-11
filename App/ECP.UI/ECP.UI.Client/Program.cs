using Blazored.LocalStorage;
using ECP.UI.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace ECP.UI.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddSingleton<NavigationManager>();
            builder.Services.AddScoped<IUserCollectionsService, UserCollectionsService>();

            await builder.Build().RunAsync();
        }
    }
}
