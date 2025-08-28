using ECP.API.Features.Artworks.Clients.ClevelandMuseum.Models;
using System.Text.Json;

namespace ECP.API.Features.Artworks.Clients.ClevelandMuseum
{
    public interface IClevelandMuseumClient
    {
        Task<List<ClevelandMuseumArtworkPreview>> GetArtworkPreview(int count);
        Task<List<ClevelandMuseumArtwork>> GetArtworksByQuery(string q);
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
            //including a description in the fields list throws a 500 error
            List<ClevelandMuseumArtworkPreview> artworks = null;
            HttpResponseMessage response = await _client.GetAsync(target_url);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<ClevelandMuseumResponsePreview>(responseContent, _jsonOptions);
                artworks = data.Data;
            }

            return artworks;
        }

        public Task<List<ClevelandMuseumArtwork>> GetArtworksByQuery(string q)
        {
            throw new NotImplementedException();
        }
    }
}
