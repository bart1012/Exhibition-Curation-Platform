using ECP.API.Features.Artworks.Clients.ChicagoArtInstitute.Models;
using ECP.API.Features.Artworks.Models;
using System.Text;
using System.Text.Json;

namespace ECP.API.Features.Artworks.Clients.ChicagoArtInstitute
{
    public interface IChicagoArtInstituteClient
    {
        Task<List<ChicagoInstArtPreview>>? GetArtworkPreviews(int count);
        Task<List<ChicagoInstArtPreview>> GetArtworksPreviewsByQuery(string q, int count, int offset);
        string UrlBuilder(ApiArtworkParameters parameters);
    }
    public class ChicagoArtClient : IChicagoArtInstituteClient
    {
        private readonly HttpClient _client;
        private readonly string BASE_URL = "https://api.artic.edu/api/v1/artworks";
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
            var parameters = new ApiArtworkParameters()
            {
                Count = count,
                PreviewsOnly = true
            };

            return await GetArtworksWithParameters(parameters);

        }

        public async Task<List<ChicagoInstArtPreview>> GetArtworksPreviewsByQuery(string q, int count, int offset)
        {
            var parameters = new ApiArtworkParameters()
            {
                Count = count,
                PreviewsOnly = true,
                Query = q,
                Offset = offset
            };

            return await GetArtworksWithParameters(parameters);
        }

        protected async Task<List<ChicagoInstArtPreview>> GetArtworksWithParameters(ApiArtworkParameters parameters)
        {
            string url = UrlBuilder(parameters);
            return await FetchArtworksAsync(url);
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

        public string UrlBuilder(ApiArtworkParameters parameters)
        {
            StringBuilder url = new();
            url.Append(BASE_URL);

            if (!string.IsNullOrEmpty(parameters.Query))
            {
                url.Append($"/search?q={parameters.Query}");
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
                url.Append("&fields=id,title,artist_titles,thumbnail,image_id");
            }

            url.Append($"&from={parameters.Offset}");

            return url.ToString();
        }
    }
}
