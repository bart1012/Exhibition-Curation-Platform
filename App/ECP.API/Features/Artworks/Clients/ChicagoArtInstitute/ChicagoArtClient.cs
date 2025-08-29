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
            return await FetchArtworksAsync(target_url);

        }

        public async Task<List<ChicagoInstArtPreview>> GetArtworksByQuery(string q, int count)
        {
            string target_url = BASE_URL + $"/api/v1/artworks/search?limit={count}&fields=id,title,artist_titles,thumbnail,image_id&q={q}";
            return await FetchArtworksAsync(target_url);

        }

        private async Task<List<ChicagoInstArtPreview>?> FetchArtworksAsync(string url)
        {
            try
            {
                var response = await _client.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<ChicagoArtInstituteResponsePreview>(responseContent, _jsonOptions);

                return data?.Data;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"An error occurred while fetching artworks: {e.Message}");
                return null;
            }
            catch (JsonException e)
            {
                Console.WriteLine($"An error occurred during JSON deserialization: {e.Message}");
                return null;
            }
        }
    }
}
