using ECP.API.Features.Artworks;
using ECP.API.Features.Artworks.Clients.ChicagoArtInstitute;
using ECP.API.Features.Artworks.Clients.ClevelandMuseum;

namespace App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IClevelandMuseumClient, ClevelandMuseumClient>();
            builder.Services.AddScoped<IChicagoArtInstituteClient, ChicagoArtClient>();
            builder.Services.AddScoped<IArtworksService, ArtworksService>();
            builder.Services.AddScoped<IArtworksRepository, ArtworksRepository>();
            builder.Services.AddTransient<IArtworkMapper, ArtworkMapper>();
            builder.Services.AddControllers();
            builder.Services.AddMemoryCache(options =>
            {
                options.SizeLimit = 3000;
                options.CompactionPercentage = 0.25;
                options.ExpirationScanFrequency = TimeSpan.FromHours(1);

            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.MapControllers();
            app.UseRouting();
            app.MapFallbackToPage("/_Host");
            app.UseAuthorization();



            app.Run();
        }
    }
}
