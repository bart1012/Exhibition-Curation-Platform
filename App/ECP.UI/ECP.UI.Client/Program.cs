using Blazored.LocalStorage;
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

            await builder.Build().RunAsync();
        }
    }
}
