using Blazored.LocalStorage;
using ECP.UI.Client.Services;
using ECP.UI.Server.Services;
using MudBlazor.Services;

namespace ECP.UI.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddMudServices();

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

            builder.Services.AddScoped<IArtworkService, ArtworkService>();
            builder.Services.AddHttpClient<ArtworkService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7102/api/");
            });
            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddScoped<IUserCollectionsService, UserCollectionsService>();



            var app = builder.Build();

            app.UseStatusCodePagesWithRedirects("/not-found");


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<ECP.UI.Server.App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            app.Run();
        }
    }
}
