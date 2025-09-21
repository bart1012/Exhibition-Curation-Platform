using ECP.API.Features.Artworks.Clients.ChicagoArtInstitute;
using ECP.API.Features.Artworks.Clients.ClevelandMuseum;
using ECP.Shared;
namespace ECP.API.Features.Artworks
{

    public interface IArtworksRepository
    {
        Task<Result<Artwork>> GetArtworkById(int id, ArtworkSource source);
        Task<Shared.Result<List<ArtworkPreview>>> GetArtworkPreviewsAsync(int count);
        Task<Shared.Result<List<ArtworkPreview>>> SearchAllArtworkPreviewsAsync(string q);
    }
    public class ArtworksRepository(IClevelandMuseumClient clevelandClient, IChicagoArtInstituteClient chicagoClient, IArtworkMapper mapper) : IArtworksRepository
    {
        private readonly IClevelandMuseumClient _clevelandClient = clevelandClient;
        private readonly IChicagoArtInstituteClient _chicagoClient = chicagoClient;
        private readonly IArtworkMapper _mapper = mapper;

        public async Task<Result<Artwork>> GetArtworkById(int id, ArtworkSource source)
        {
            try
            {
                Artwork artwork = null;

                switch (source)
                {
                    case ArtworkSource.CLEVELAND_MUSEUM: artwork = _mapper.FromCleveland(await _clevelandClient.GetArtworkById(id)); break;
                    case ArtworkSource.CHICAGO_ART_INSTITUTE: artwork = _mapper.FromChicago(await _chicagoClient.GetArtworkById(id)); break;
                }

                return Result<Artwork>.Success(artwork);
            }
            catch (HttpRequestException ex)
            {
                return Shared.Result<Artwork>.Failure($"Failed to fetch artworks from {ex.Source}: {ex.Message}", ex.StatusCode);
            }

        }

        public async Task<Shared.Result<List<ArtworkPreview>>> GetArtworkPreviewsAsync(int count)
        {
            try
            {
                int collectionOneAmount = count / 2;
                int collectionTwoAmount = count - collectionOneAmount;

                var clevelandTask = _clevelandClient.GetArtworkPreviews(collectionOneAmount);
                var chicagoTask = _chicagoClient.GetArtworkPreviews(collectionTwoAmount);

                await Task.WhenAll(clevelandTask, chicagoTask);

                var clevelandResults = clevelandTask.Result;
                var chicagoResults = chicagoTask.Result;

                var artworkPreviews = new List<ArtworkPreview>();

                artworkPreviews.AddRange(clevelandResults.Select(a => _mapper.FromClevelandPreview(a)).Where(a => a.Thumbnail != null).ToList());
                artworkPreviews.AddRange(chicagoResults.Select(a => _mapper.FromChicagoPreview(a)).Where(a => a.Thumbnail != null).ToList());

                return Shared.Result<List<ArtworkPreview>>.Success(artworkPreviews);
            }
            catch (HttpRequestException ex)
            {
                return Shared.Result<List<ArtworkPreview>>.Failure($"Failed to fetch artworks from {ex.Source}: {ex.Message}", ex.StatusCode);
            }

        }

        public async Task<Result<List<ArtworkPreview>>> SearchAllArtworkPreviewsAsync(string q)
        {
            try
            {
                var clevelandTask = _clevelandClient.GetArtworkPreviewsByQuery(q);
                var chicagoTask = _chicagoClient.SearchArtworkPreviewsByQuery(q);

                await Task.WhenAll(clevelandTask, chicagoTask);

                var clevelandResults = clevelandTask.Result;
                var chicagoResults = chicagoTask.Result;
                var artworkPreviews = new List<ArtworkPreview>();



                var filteredClevelandList = clevelandResults.Select(a => _mapper.FromClevelandPreview(a)).Where(a => a.Thumbnail != null).ToList();
                var filteredChicagoList = chicagoResults.Select(a => _mapper.FromChicagoPreview(a)).Where(a => a.Thumbnail != null).ToList();

                Console.WriteLine($"Cleveland total: {filteredClevelandList.Count()}");
                Console.WriteLine($"Chicago total: {filteredChicagoList.Count()}");



                artworkPreviews.AddRange(filteredClevelandList);
                artworkPreviews.AddRange(filteredChicagoList);



                return Shared.Result<List<ArtworkPreview>>.Success(artworkPreviews);
            }
            catch (HttpRequestException ex)
            {
                return Shared.Result<List<ArtworkPreview>>.Failure($"Failed to fetch artworks from {ex.Source}: {ex.Message}", ex.StatusCode);
            }
        }
    }
}
