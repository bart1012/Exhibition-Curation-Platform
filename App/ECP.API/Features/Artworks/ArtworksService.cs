// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using ECP.Shared;


namespace ECP.API.Features.Artworks
{
    public interface IArtworksService
    {
        Task<Shared.Result<List<ArtworkPreview>>> GetArtworkPreviewAsync(int count);
        Task<Shared.Result<List<ArtworkPreview>>> GetArtworkPreviewByQueryAsync(string q, int count, int offset);
    }
    public class ArtworksService(IArtworksRepository artworksRepository) : IArtworksService
    {
        private readonly IArtworksRepository _artworksRepository = artworksRepository;
        public async Task<Shared.Result<List<ArtworkPreview>>> GetArtworkPreviewAsync(int count)
        {
            return await _artworksRepository.GetArtworkPreviewAsync(count);
        }

        public async Task<Result<List<ArtworkPreview>>> GetArtworkPreviewByQueryAsync(string q, int count, int offset)
        {
            return await _artworksRepository.GetArtworkPreviewByQueryAsync(q, count, offset);

        }
    }
}
