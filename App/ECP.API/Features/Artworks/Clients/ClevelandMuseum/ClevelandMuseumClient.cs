using ECP.API.Features.Artworks.Clients.ChicagoArtInstitute.Models;
using ECP.API.Features.Artworks.Clients.ClevelandMuseum.Models;
using System.Text;
using System.Text.Json;

namespace ECP.API.Features.Artworks.Clients.ClevelandMuseum
{
    public interface IClevelandMuseumClient
    {
        Task<ClevelandArtwork?> GetArtworkById(int id);
        Task<List<ClevelandArtworkPreview>> GetArtworkPreviews(int count);
        Task<List<ClevelandArtworkPreview>> GetArtworkPreviewsByQuery(string q);
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

        public async Task<ClevelandArtwork?> GetArtworkById(int id)
        {
            string url = $"https://openaccess-api.clevelandart.org/api/artworks/{id}";
            var apiResponse = await FetchObjectAsync(url);
            return apiResponse;
        }

        public async Task<List<ClevelandArtworkPreview>?> GetArtworkPreviews(int count = 25)
        {
            var parameters = new ChicagoApiParameters()
            {
                Count = count,
                PreviewsOnly = true
            };
            string url = UrlBuilder(parameters);
            return await FetchCollectionAsync(url);
        }

        public async Task<List<ClevelandArtworkPreview>> GetArtworkPreviewsByQuery(string q)
        {
            var parameters = new ChicagoApiParameters()
            {
                Count = 0,
                PreviewsOnly = true,
                Query = q,
                Offset = 0
            };
            string url = UrlBuilder(parameters);
            return await FetchCollectionAsync(url);

        }

        private string UrlBuilder(ChicagoApiParameters parameters)
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
            url.Append("&has_image=1");

            if (parameters.PreviewsOnly)
            {
                url.Append("&fields=department,collection,creation_date_earliest,creation_date_latest,sortable_date,technique,support_materials,id,title,creators,images,creation_date,type,culture");

            }


            url.Append($"&skip={parameters.Offset}");

            return url.ToString();
        }

        private async Task<List<ClevelandArtworkPreview>?> FetchCollectionAsync(string url)
        {
            Console.WriteLine($"\n\n\n\nURL:{url}\n\n\n");
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
                Console.WriteLine($"An error occurred while fetching artworks from Cleveland API: {e.Message}");
                return null;
            }
            catch (JsonException e)
            {
                Console.WriteLine($"An error occurred during JSON deserialization: {e.Message}");
                return null;
            }
        }

        private async Task<ClevelandArtwork?> FetchObjectAsync(string url)
        {
            try
            {
                var response = await _client.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var parsedResponse = JsonSerializer.Deserialize<ClevelandObjectResponse<ClevelandArtwork>>(responseContent, _jsonOptions);
                return parsedResponse.Data;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"An error occurred while fetching artwork from Cleveland API: {e.Message}");
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
