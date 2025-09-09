using ECP.Shared;

namespace ECP.UI.Server.Services
{
    public interface IUserCollectionsService
    {
        Task<List<ExhibitionCollection>> GetCollectionsAsync();
        Task CreateCollectionAsync(ExhibitionCollection collection);
        Task AddArtworkToCollectionAsync(string collectionId, ArtworkPreview artwork);
        Task RemoveArtworkFromCollectionAsync(string collectionId, string artworkId);
    }
    public class UserCollectionsService : IUserCollectionsService
    {
        public Task AddArtworkToCollectionAsync(string collectionId, ArtworkPreview artwork)
        {
            throw new NotImplementedException();
        }

        public Task CreateCollectionAsync(ExhibitionCollection collection)
        {
            throw new NotImplementedException();
        }

        public Task<List<ExhibitionCollection>> GetCollectionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task RemoveArtworkFromCollectionAsync(string collectionId, string artworkId)
        {
            throw new NotImplementedException();
        }
    }
}
