using ECP.Shared;
using System.Net;
using System.Text.Json;

namespace ECP.UI.Server.Services
{
    public interface IArtworkService
    {
        Task<Result<PaginatedResponse<ArtworkPreview>>> GetArtworksAsync(int count, int resultsPerPage, int pageNum);
        Task<Result<PaginatedResponse<ArtworkPreview>>> SearchArtworksByQueryAsync(string q, int resultsPerPage, int pageNum);
    }
    public class ArtworkService(HttpClient client) : IArtworkService
    {
        private readonly HttpClient _httpClient = client;
        public async Task<Result<PaginatedResponse<ArtworkPreview>>> GetArtworksAsync(int count, int resultsPerPage, int pageNum)
        {
            return await ExecuteApiCallAsync<PaginatedResponse<ArtworkPreview>>($"artworks/previews?count={count}&results_per_page={resultsPerPage}&page_num={pageNum}");
        }

        public async Task<Result<PaginatedResponse<ArtworkPreview>>> SearchArtworksByQueryAsync(string q, int resultsPerPage, int pageNum)
        {
            return await ExecuteApiCallAsync<PaginatedResponse<ArtworkPreview>>($"artworks/previews/search?&q={q}&results_per_page={resultsPerPage}&page_num={pageNum}");
        }

        protected async Task<Result<T>> ExecuteApiCallAsync<T>(string url)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return Result<T>.Failure($"Http Error: {response.StatusCode}", HttpStatusCode.ServiceUnavailable);
                }

                string httpContent = await response.Content.ReadAsStringAsync();
                var list = JsonSerializer.Deserialize<T>(httpContent, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

                return Result<T>.Success(list);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Http Request Failed: {ex.Message}");
                return Result<T>.Failure(ex.Message, HttpStatusCode.ServiceUnavailable);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unknown Exception: {ex.Message}");
                return Result<T>.Failure(ex.Message, HttpStatusCode.InternalServerError);
            }

        }

    }
}
