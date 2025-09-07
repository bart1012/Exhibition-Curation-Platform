using ECP.API.Features.Artworks.Clients.ChicagoArtInstitute.Models;
using ECP.API.Features.Artworks.Clients.ClevelandMuseum.Models;
using System.Text;
using System.Text.Json;

namespace ECP.API.Features.Artworks.Clients.ClevelandMuseum
{
    public interface IClevelandMuseumClient
    {
        Task<List<ClevelandMuseumArtworkPreview>> GetArtworkPreviews(int count);
        Task<List<ClevelandMuseumArtworkPreview>> GetArtworkPreviewsByQuery(string q);
        string UrlBuilder(ChicagoApiParameters parameters);
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

        public async Task<List<ClevelandMuseumArtworkPreview>?> GetArtworkPreviews(int count = 25)
        {
            var parameters = new ChicagoApiParameters()
            {
                Count = count,
                PreviewsOnly = true
            };
            return await GetArtworksWithParameters(parameters);
        }

        public async Task<List<ClevelandMuseumArtworkPreview>> GetArtworkPreviewsByQuery(string q)
        {
            var parameters = new ChicagoApiParameters()
            {
                Count = 0,
                PreviewsOnly = true,
                Query = q,
                Offset = 0
            };
            return await GetArtworksWithParameters(parameters);

        }

        public string UrlBuilder(ChicagoApiParameters parameters)
        {
            StringBuilder url = new();
            url.Append(BASE_URL + "artworks");

            if (!string.IsNullOrEmpty(parameters.Query))
            {
                url.Append($"?q={parameters.Query}");
            }
            else
            {
                url.Append("?");
            }

            if (parameters.Count != 0)
            {
                url.Append($"&limit={parameters.Count}");
            }

            if (parameters.PreviewsOnly)
            {
                url.Append("&fields=id,title,creators,images,creation_date&has_image=1");
            }

            url.Append($"&skip={parameters.Offset}");

            return url.ToString();
        }

        protected async Task<List<ClevelandMuseumArtworkPreview>> GetArtworksWithParameters(ChicagoApiParameters parameters)
        {
            string url = UrlBuilder(parameters);
            return await FetchArtworksAsync(url);
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
