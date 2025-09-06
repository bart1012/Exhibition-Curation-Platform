using ECP.API.Features.Artworks.Clients.ChicagoArtInstitute;
using ECP.API.Features.Artworks.Clients.ClevelandMuseum;
using ECP.Shared;
namespace ECP.API.Features.Artworks
{

    public interface IArtworksRepository
    {
        Task<Shared.Result<List<ArtworkPreview>>> GetArtworkPreviewsAsync(int count);
        Task<Shared.Result<List<ArtworkPreview>>> SearchAllArtworkPreviewsAsync(string q);
    }
    public class ArtworksRepository(IClevelandMuseumClient clevelandClient, IChicagoArtInstituteClient chicagoClient, IArtworkMapper mapper) : IArtworksRepository
    {
        private readonly IClevelandMuseumClient _clevelandClient = clevelandClient;
        private readonly IChicagoArtInstituteClient _chicagoClient = chicagoClient;
        private readonly IArtworkMapper _mapper = mapper;
        public async Task<Shared.Result<List<ArtworkPreview>>> GetArtworkPreviewsAsync(int count)
        {
            try
            {
                var clevelandTask = _clevelandClient.GetArtworkPreviews(count);
                var chicagoTask = _chicagoClient.GetArtworkPreviews(count);

                await Task.WhenAll(clevelandTask, chicagoTask);

                var clevelandResults = clevelandTask.Result;
                var chicagoResults = chicagoTask.Result;
                var artworkPreviews = new List<ArtworkPreview>();

                artworkPreviews.AddRange(clevelandResults.Select(a => _mapper.FromClevelandPreview(a)).Where(a => a.WebImage != null).ToList());
                artworkPreviews.AddRange(chicagoResults.Select(a => _mapper.FromChicagoPreview(a)).Where(a => a.WebImage != null).ToList());

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



                var filteredClevelandList = clevelandResults.Select(a => _mapper.FromClevelandPreview(a)).Where(a => a.WebImage != null).ToList();
                var filteredChicagoList = chicagoResults.Select(a => _mapper.FromChicagoPreview(a)).Where(a => a.WebImage != null).ToList();



                artworkPreviews.AddRange(filteredClevelandList);
                artworkPreviews.AddRange(filteredChicagoList);

                Console.WriteLine($"""
                    Cleveland results unfiltered: {clevelandResults.Count}
                    Cleveland results filtered: {filteredClevelandList.Count}
                    Chicago results unfiltered: {chicagoResults.Count}
                    Chicago results filtered: {filteredChicagoList.Count}
                    Total: {artworkPreviews.Count}
                    """);

                return Shared.Result<List<ArtworkPreview>>.Success(artworkPreviews);
            }
            catch (HttpRequestException ex)
            {
                return Shared.Result<List<ArtworkPreview>>.Failure($"Failed to fetch artworks from {ex.Source}: {ex.Message}", ex.StatusCode);
            }
        }
    }
}
