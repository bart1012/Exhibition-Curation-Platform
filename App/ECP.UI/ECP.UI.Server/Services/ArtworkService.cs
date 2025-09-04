using ECP.Shared;
using ECP.UI.Server.Components;
using System.Net;
using System.Text.Json;

namespace ECP.UI.Server.Services
{
    public interface IArtworkService
    {
        Task<ServiceResponse<List<ArtworkPreview>>> GetArtworksAsync(int count);
        Task<ServiceResponse<List<ArtworkPreview>>> GetArtworksByQueryAsync(int count, string q, int offset);
    }
    public class ArtworkService(HttpClient client) : IArtworkService
    {
        private readonly HttpClient _httpClient = client;
        public async Task<ServiceResponse<List<ArtworkPreview>>> GetArtworksAsync(int count)
        {
            return await ExecuteApiCallAsync<List<ArtworkPreview>>($"artworks/previews?count={count}");
        }

        public async Task<ServiceResponse<List<ArtworkPreview>>> GetArtworksByQueryAsync(int count, string q, int offset)
        {
            return await ExecuteApiCallAsync<List<ArtworkPreview>>($"artworks/previews/search?count={count}&q={q}&offset={offset}");
        }

        protected async Task<ServiceResponse<T>> ExecuteApiCallAsync<T>(string url)
        {
            var result = new ServiceResponse<T>();
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                result.StatusCode = response.StatusCode;

                if (!response.IsSuccessStatusCode)
                {
                    result.Success = false;
                    result.Message = $"Http Error: {response.StatusCode}";
                    return result;
                }
                result.Success = true;
                string httpContent = await response.Content.ReadAsStringAsync();
                var list = JsonSerializer.Deserialize<T>(httpContent, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

                result.Data = list;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Http Request Failed: {ex.Message}");

                result.Success = false;
                result.Message = ex.Message;
                result.StatusCode = HttpStatusCode.ServiceUnavailable;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unknown Exception: {ex.Message}");
                result.Success = false;
                result.Message = ex.Message;
                result.StatusCode = HttpStatusCode.InternalServerError;
            }
            return result;
        }

    }
}
