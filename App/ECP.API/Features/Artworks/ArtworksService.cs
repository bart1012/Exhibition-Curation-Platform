// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using ECP.API.Utils;
using ECP.Shared;


namespace ECP.API.Features.Artworks
{
    public interface IArtworksService
    {
        Task<Shared.Result<PaginatedResponse<ArtworkPreview>>> GetArtworkPreviewsAsync(int count, int resultsPerPage, int pageNum);
        Task<Shared.Result<PaginatedResponse<ArtworkPreview>>> SearchAllArtworkPreviewsAsync(string q, int resultsPerPage, int pageNum);
    }
    public class ArtworksService(IArtworksRepository artworksRepository) : IArtworksService
    {
        private readonly IArtworksRepository _artworksRepository = artworksRepository;
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
            Result<List<ArtworkPreview>> repositoryResponse = await _artworksRepository.SearchAllArtworkPreviewsAsync(q);
            if (!repositoryResponse.IsSuccess)
            {
                return Shared.Result<PaginatedResponse<ArtworkPreview>>.Failure(repositoryResponse.Message, repositoryResponse.StatusCode);
            }
            var allArtworks = repositoryResponse.Value ?? new List<ArtworkPreview>();

            var paginatedResponse = PaginatedResponseBuilder<ArtworkPreview>.Build(allArtworks, resultsPerPage, pageNum);

            return Shared.Result<PaginatedResponse<ArtworkPreview>>.Success(paginatedResponse);

        }


    }
}
