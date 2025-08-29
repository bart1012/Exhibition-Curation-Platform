using ECP.API.Features.Artworks.Clients.ChicagoArtInstitute.Models;
using System.Text.Json;

namespace ECP.API.Features.Artworks.Clients.ChicagoArtInstitute
{
    public interface IChicagoArtInstituteClient
    {
        Task<List<ChicagoInstArtPreview>>? GetArtworkPreviews(int count);
        Task<List<ChicagoInstArtPreview>> GetArtworksByQuery(string q, int count);

    }
    public class ChicagoArtClient : IChicagoArtInstituteClient
    {
        private readonly HttpClient _client;
        private readonly string BASE_URL = "https://api.artic.edu";
        private readonly JsonSerializerOptions _jsonOptions;

        public ChicagoArtClient()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("User-Agent", "ECP/1.0");
            _jsonOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<List<ChicagoInstArtPreview>>? GetArtworkPreviews(int count = 25)
        {
            string target_url = BASE_URL + $"/api/v1/artworks?limit={count}&fields=id,title,artist_titles,thumbnail,image_id";
            List<ChicagoInstArtPreview> artworks = null;
            HttpResponseMessage response = await _client.GetAsync(target_url);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Chicago Api: success");
                var responseContent = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<ChicagoArtInstituteResponsePreview>(responseContent, _jsonOptions);
                artworks = data.Data;
            }

            return artworks;
        }

        public async Task<List<ChicagoInstArtPreview>> GetArtworksByQuery(string q, int count)
        {
            string target_url = BASE_URL + $"/api/v1/artworks/search?limit={count}&fields=id,title,artist_titles,thumbnail,image_id&q={q}";
            List<ChicagoInstArtPreview> artworks = null;
            HttpResponseMessage response = await _client.GetAsync(target_url);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Chicago Api: success");
                var responseContent = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<ChicagoArtInstituteResponsePreview>(responseContent, _jsonOptions);
                artworks = data.Data;
            }

            return artworks;
        }
    }
}
