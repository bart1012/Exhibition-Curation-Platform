using ECP.Shared;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ECP.UI.Server.Services
{
    public interface IArtworkService
    {
        Task<Result<PaginatedResponse<ArtworkPreview>>> GetArtworkPreviewsAsync(int count, int resultsPerPage, int pageNum);

        Task<Result<Artwork>> GetArtworkByIdAsync(int id, int source);

        Task<Result<PaginatedResponse<ArtworkPreview>>> SearchArtworksByQueryAsync(string q, int resultsPerPage, int pageNum, string? sortOptions = null, string? filterOptions = null);
    }
    public class ArtworkService(HttpClient client) : IArtworkService
    {
        private readonly HttpClient _httpClient = client;
        private readonly string BASE_URL = "https://exhibition-api-bart1012-fdcgghfubqh2dyhr.ukwest-01.azurewebsites.net/api/artworks";

        public async Task<Result<Artwork>> GetArtworkByIdAsync(int id, int source)
        {
            string url = $"{BASE_URL}?id={id}&source={source}";
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return Result<Artwork>.Failure($"Http Error: {response.StatusCode}", response.StatusCode);
                }

                string httpContent = await response.Content.ReadAsStringAsync();
                var artwork = JsonSerializer.Deserialize<Artwork>(httpContent, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

                return Result<Artwork>.Success(artwork);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Http Request Failed: {ex.Message}");
                return Result<Artwork>.Failure(ex.Message, HttpStatusCode.ServiceUnavailable);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unknown Exception: {ex.Message}");
                return Result<Artwork>.Failure(ex.Message, HttpStatusCode.InternalServerError);
            }

        }

        public async Task<Result<PaginatedResponse<ArtworkPreview>>> GetArtworkPreviewsAsync(int count, int resultsPerPage, int pageNum)
        {
            string url = $"{BASE_URL}/previews?count={count}&results_per_page={resultsPerPage}&page_num={pageNum}";
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return Result<PaginatedResponse<ArtworkPreview>>.Failure($"Http Error: {response.StatusCode}", HttpStatusCode.ServiceUnavailable);
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return Result<PaginatedResponse<ArtworkPreview>>.Success(new PaginatedResponse<ArtworkPreview>());
                }

                string httpContent = await response.Content.ReadAsStringAsync();
                var list = JsonSerializer.Deserialize<PaginatedResponse<ArtworkPreview>>(httpContent, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

                return Result<PaginatedResponse<ArtworkPreview>>.Success(list);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Http Request Failed: {ex.Message}");
                return Result<PaginatedResponse<ArtworkPreview>>.Failure(ex.Message, HttpStatusCode.ServiceUnavailable);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unknown Exception: {ex.Message}");
                return Result<PaginatedResponse<ArtworkPreview>>.Failure(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<PaginatedResponse<ArtworkPreview>>> SearchArtworksByQueryAsync(string q, int resultsPerPage, int pageNum, string? sortOptions = null, string? filterOptions = null)
        {
            StringBuilder url = new($"{BASE_URL}/previews/search?&q={q}&limit={resultsPerPage}&p={pageNum}");

            if (!string.IsNullOrEmpty(sortOptions))
            {
                url.Append(sortOptions);
            }
            if (!string.IsNullOrEmpty(filterOptions))
            {
                url.Append(filterOptions);
            }

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url.ToString());

                if (!response.IsSuccessStatusCode)
                {
                    return Result<PaginatedResponse<ArtworkPreview>>.Failure($"Http Error: {response.StatusCode}", HttpStatusCode.ServiceUnavailable);
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return Result<PaginatedResponse<ArtworkPreview>>.Success(new PaginatedResponse<ArtworkPreview>() { Data = new List<ArtworkPreview>() });
                }

                string httpContent = await response.Content.ReadAsStringAsync();
                var list = JsonSerializer.Deserialize<PaginatedResponse<ArtworkPreview>>(httpContent, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

                return Result<PaginatedResponse<ArtworkPreview>>.Success(list);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Http Request Failed: {ex.Message}");
                return Result<PaginatedResponse<ArtworkPreview>>.Failure(ex.Message, HttpStatusCode.ServiceUnavailable);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unknown Exception: {ex.Message}");
                return Result<PaginatedResponse<ArtworkPreview>>.Failure(ex.Message, HttpStatusCode.InternalServerError);
            }

        }



    }
}
