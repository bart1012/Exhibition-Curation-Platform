using ECP.API.Features.Artworks.Clients.ChicagoArtInstitute.Models;
using System.Text;
using System.Text.Json;

namespace ECP.API.Features.Artworks.Clients.ChicagoArtInstitute
{
    public interface IChicagoArtInstituteClient
    {
        Task<ChicagoArtwork?> GetArtworkById(int id);
        Task<List<ChicagoArtworkPreview>>? GetArtworkPreviews(int count);
        Task<List<ChicagoArtworkPreview>> SearchArtworkPreviewsByQuery(string q);
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

        public async Task<List<ChicagoArtworkPreview>>? GetArtworkPreviews(int count = 25)
        {
            var parameters = new ChicagoApiParameters()
            {
                Count = count,
                PreviewsOnly = true
            };

            string url = UrlBuilder(parameters);
            var result = await FetchChicagoCollectionApiResponseAsync<ChicagoArtworkPreview>(url);
            return result.Data;

        }

        public async Task<List<ChicagoArtworkPreview>> SearchArtworkPreviewsByQuery(string q)
        {
            var artworks = new List<ChicagoArtworkPreview>();

            var parameters = new ChicagoApiParameters()
            {
                Count = 100,
                PreviewsOnly = true,
                Query = q,
                Page = 1
            };
            string url = UrlBuilder(parameters);

            ChicagoCollectionApiResponse<ChicagoArtworkPreview> firstResponse = await FetchChicagoCollectionApiResponseAsync<ChicagoArtworkPreview>(url);

            artworks.AddRange(firstResponse.Data);

            if (firstResponse.Info.Pages > 1)
            {
                while (parameters.Page < firstResponse.Info.Pages)
                {
                    parameters.Page++;
                    url = UrlBuilder(parameters);
                    ChicagoCollectionApiResponse<ChicagoArtworkPreview> response = await FetchChicagoCollectionApiResponseAsync<ChicagoArtworkPreview>(url);

                    if (response != null)
                    {
                        artworks.AddRange(response.Data);
                    }

                }

            }

            return artworks;
        }

        public async Task<ChicagoArtwork?> GetArtworkById(int id)
        {
            string url = $"https://api.artic.edu/api/v1/artworks/{id}";
            var apiResponse = await FetchChicagoObjectApiResponseAsync<ChicagoArtwork>(url);
            return apiResponse?.Data;
        }



        private async Task<ChicagoObjectApiResponse<T>> FetchChicagoObjectApiResponseAsync<T>(string url)
        {
            try
            {
                var response = await _client.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var apiResponseModel = JsonSerializer.Deserialize<ChicagoObjectApiResponse<T>>(responseContent, _jsonOptions);

                return apiResponseModel;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"An error occurred while fetching artwork: {e.Message}");
                return null;
            }
            catch (JsonException e)
            {
                Console.WriteLine($"An error occurred during JSON deserialization: {e.Message}");
                return null;
            }
        }

        private async Task<ChicagoCollectionApiResponse<T>> FetchChicagoCollectionApiResponseAsync<T>(string url)
        {
            try
            {
                var response = await _client.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var apiResponseModel = JsonSerializer.Deserialize<ChicagoCollectionApiResponse<T>>(responseContent, _jsonOptions);

                return apiResponseModel;
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

        private string UrlBuilder(ChicagoApiParameters parameters)
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
                url.Append($"&size={parameters.Count}");
            }

            if (parameters.PreviewsOnly)
            {
                url.Append("&fields=id,title,artist_titles,thumbnail,image_id,date_start,date_end");
            }

            if (parameters.Page != 0)
            {
                url.Append($"&page={parameters.Page}");
            }


            return url.ToString();
        }


    }
}
