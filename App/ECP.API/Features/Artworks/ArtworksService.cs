// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using ECP.API.Utils;
using ECP.Shared;
using Microsoft.Extensions.Caching.Memory;


namespace ECP.API.Features.Artworks
{
    public interface IArtworksService
    {
        Task<Shared.Result<PaginatedResponse<ArtworkPreview>>> GetArtworkPreviewsAsync(int count, int resultsPerPage, int pageNum);
        Task<Shared.Result<PaginatedResponse<ArtworkPreview>>> SearchAllArtworkPreviewsAsync(string q, int resultsPerPage, int pageNum);
    }
    public class ArtworksService(IArtworksRepository artworksRepository, IMemoryCache memoryCache) : IArtworksService
    {
        private readonly IArtworksRepository _artworksRepository = artworksRepository;
        private readonly IMemoryCache _cache = memoryCache;
        public async Task<Shared.Result<PaginatedResponse<ArtworkPreview>>> GetArtworkPreviewsAsync(int count, int resultsPerPage, int pageNum)
        {
            Result<List<ArtworkPreview>> repositoryResponse = await _artworksRepository.GetArtworkPreviewsAsync(count);
            if (!repositoryResponse.IsSuccess)
            {
                return Shared.Result<PaginatedResponse<ArtworkPreview>>.Failure(repositoryResponse.Message, repositoryResponse.StatusCode);
            }
            var allArtworks = repositoryResponse.Value ?? new List<ArtworkPreview>();

            var paginatedResponse = PaginatedResponseBuilder<ArtworkPreview>.Build(allArtworks, resultsPerPage, pageNum);

            return Shared.Result<PaginatedResponse<ArtworkPreview>>.Success(paginatedResponse);

        }

        public async Task<Shared.Result<PaginatedResponse<ArtworkPreview>>> SearchAllArtworkPreviewsAsync(string q, int resultsPerPage, int pageNum)
        {
            List<ArtworkPreview> artworks = null;

            if (!_cache.TryGetValue(q, out artworks))
            {
                Result<List<ArtworkPreview>> repositoryResponse = await _artworksRepository.SearchAllArtworkPreviewsAsync(q);
                if (!repositoryResponse.IsSuccess)
                {
                    return Shared.Result<PaginatedResponse<ArtworkPreview>>.Failure(repositoryResponse.Message, repositoryResponse.StatusCode);
                }

                if (repositoryResponse.Value != null && repositoryResponse.Value.Count > 0)
                {
                    MemoryCacheEntryOptions options = new()
                    {
                        Size = repositoryResponse.Value.Count,
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
                    };
                    _cache.Set(q, repositoryResponse.Value, options);
                }

            }


            var allArtworks = artworks ?? new List<ArtworkPreview>();

            var paginatedResponse = PaginatedResponseBuilder<ArtworkPreview>.Build(allArtworks, resultsPerPage, pageNum);

            return Shared.Result<PaginatedResponse<ArtworkPreview>>.Success(paginatedResponse);

        }

    }
}
