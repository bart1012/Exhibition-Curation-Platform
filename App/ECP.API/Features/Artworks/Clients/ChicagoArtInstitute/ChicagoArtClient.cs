using ECP.API.Features.Artworks.Clients.ChicagoArtInstitute.Models;
using System.Text;
using System.Text.Json;

namespace ECP.API.Features.Artworks.Clients.ChicagoArtInstitute
{
    public interface IChicagoArtInstituteClient
    {
        Task<List<ChicagoArtPreview>>? GetArtworkPreviews(int count);
        Task<List<ChicagoArtPreview>> SearchArtworkPreviewsByQuery(string q);
        string UrlBuilder(ChicagoApiParameters parameters);
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

        public async Task<List<ChicagoArtPreview>>? GetArtworkPreviews(int count = 25)
        {
            var parameters = new ChicagoApiParameters()
            {
                Count = count,
                PreviewsOnly = true
            };

            var apiResponse = await GetResponseWithParameters<ChicagoArtPreview>(parameters);
            return apiResponse.Data;

        }

        public async Task<List<ChicagoArtPreview>> SearchArtworkPreviewsByQuery(string q)
        {
            var artworks = new List<ChicagoArtPreview>();

            var parameters = new ChicagoApiParameters()
            {
                Count = 100,
                PreviewsOnly = true,
                Query = q,
                Page = 1
            };

            ChicagoApiResponse<ChicagoArtPreview> firstResponse = await GetResponseWithParameters<ChicagoArtPreview>(parameters);

            artworks.AddRange(firstResponse.Data);

            if (firstResponse.Info.Pages > 1)
            {
                while (parameters.Page < firstResponse.Info.Pages)
                {
                    parameters.Page++;
                    ChicagoApiResponse<ChicagoArtPreview> response = await GetResponseWithParameters<ChicagoArtPreview>(parameters);

                    if (response != null)
                    {
                        artworks.AddRange(response.Data);
                    }

                }

            }

            return artworks;
        }

        protected async Task<ChicagoApiResponse<T>> GetResponseWithParameters<T>(ChicagoApiParameters parameters)
        {
            string url = UrlBuilder(parameters);
            return await FetchChicagoApiResponseAsync<T>(url);
        }

        private async Task<ChicagoApiResponse<T>> FetchChicagoApiResponseAsync<T>(string url)
        {
            try
            {
                var response = await _client.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var apiResponseModel = JsonSerializer.Deserialize<ChicagoApiResponse<T>>(responseContent, _jsonOptions);

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

        public string UrlBuilder(ChicagoApiParameters parameters)
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
