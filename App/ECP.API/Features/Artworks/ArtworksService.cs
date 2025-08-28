// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using ECP.API.Features.Artworks.Models;


namespace ECP.API.Features.Artworks
{
    public interface IArtworksService
    {
        Task<Shared.Result<List<ArtworkPreview>>> GetArtworkPreviewAsync(int count);
    }
    public class ArtworksService(IArtworksRepository artworksRepository) : IArtworksService
    {
        private readonly IArtworksRepository _artworksRepository = artworksRepository;
        public async Task<Shared.Result<List<ArtworkPreview>>> GetArtworkPreviewAsync(int count)
        {
            return await _artworksRepository.GetArtworkPreviewAsync(count);
        }
    }
}
