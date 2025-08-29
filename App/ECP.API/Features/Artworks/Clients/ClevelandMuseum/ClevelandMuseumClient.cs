using ECP.API.Features.Artworks.Clients.ClevelandMuseum.Models;
using System.Text.Json;

namespace ECP.API.Features.Artworks.Clients.ClevelandMuseum
{
    public interface IClevelandMuseumClient
    {
        Task<List<ClevelandMuseumArtworkPreview>> GetArtworkPreview(int count);
        Task<List<ClevelandMuseumArtworkPreview>> GetArtworksByQuery(string q, int count);
    }

    public class ClevelandMuseumClient : IClevelandMuseumClient
    {
        private readonly HttpClient _client;
        private readonly string BASE_URL = "https://openaccess-api.clevelandart.org/api/";
        private readonly JsonSerializerOptions _jsonOptions;

        public ClevelandMuseumClient()
        {
            _client = new HttpClient();
            _jsonOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<List<ClevelandMuseumArtworkPreview>?> GetArtworkPreview(int count = 25)
        {
            string target_url = BASE_URL + "artworks" + $"/?limit={count}&fields=id,title,creators,images";
            return await FetchArtworksAsync(target_url);
        }

        public async Task<List<ClevelandMuseumArtworkPreview>> GetArtworksByQuery(string q, int count)
        {
            string target_url = BASE_URL + "artworks" + $"/?limit={count}&fields=id,title,creators,images&q={q}";
            return await FetchArtworksAsync(target_url);
        }

        private async Task<List<ClevelandMuseumArtworkPreview>?> FetchArtworksAsync(string url)
        {
            try
            {
                var response = await _client.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<ClevelandMuseumResponsePreview>(responseContent, _jsonOptions);

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
