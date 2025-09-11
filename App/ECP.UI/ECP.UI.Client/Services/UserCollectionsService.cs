using Blazored.LocalStorage;
using ECP.Shared;

namespace ECP.UI.Client.Services
{
    public interface IUserCollectionsService
    {
        Task<Result<UserCollections>> GetCollectionsAsync();
        Task<Result> CreateCollectionAsync(Collection collection);
        Task<Result> AddArtworkToCollectionAsync(string collectionId, Artwork artwork);
        Task<Result> RemoveArtworkFromCollectionAsync(string collectionId, string artworkId);
    }
    public class UserCollectionsService(ILocalStorageService localStorage) : IUserCollectionsService
    {
        private readonly ILocalStorageService _localStorage = localStorage;

        public async Task<Result> AddArtworkToCollectionAsync(string collectionId, Artwork artwork)
        {
            var parsedUserCollections = await GetOrCreateUserCollectionsAsync();
            var collection = parsedUserCollections.Collections.FirstOrDefault(c => c.Id == collectionId);
            if (collection != null)
            {
                bool alreadyExists = collection.Artworks.Any(a => a.Id == artwork.Id);
                if (alreadyExists) return Result.Failure($"An artwork with an ID of {artwork.Id} already exists in '{collection.Name}'");
                collection.Artworks.Add(artwork);
                await _localStorage.SetItemAsync<UserCollections>("UserCollections", parsedUserCollections);
                return Result.Success();
            }
            return Result.Failure($"The collection with an ID of {collectionId} does not exist.");

        }

        public async Task<Result> CreateCollectionAsync(Collection collection)
        {
            try
            {
                var userCollections = await GetOrCreateUserCollectionsAsync();

                userCollections.Collections.Add(collection);
                await _localStorage.SetItemAsync<UserCollections>("UserCollections", userCollections);
                return Result.Success();

            }
            catch (Exception e)
            {
                Console.WriteLine($"An unknown exception has occured while trying to write to local storage: {e.Message}");
                return Result.Failure(e.Message);
            }

        }

        public async Task<Result<UserCollections>> GetCollectionsAsync()
        {
            var collections = await _localStorage.GetItemAsync<UserCollections>("UserCollections");
            if (collections == null) return Result<UserCollections>.Failure("There are no existing collections");
            return Result<UserCollections>.Success(collections);
        }

        public async Task<Result> RemoveArtworkFromCollectionAsync(string collectionId, string artworkId)
        {
            var parsedUserCollections = await GetOrCreateUserCollectionsAsync();
            var collection = parsedUserCollections.Collections.FirstOrDefault(c => c.Id == collectionId);
            if (collection != null)
            {
                int numRemoved = collection.Artworks.RemoveAll(a => string.Equals(a.Id, artworkId));
                if (numRemoved == 0) return Result.Failure($"Could not find an artwork with ID: {artworkId}.");
                await _localStorage.SetItemAsync<UserCollections>("UserCollections", parsedUserCollections);
                return Result.Success();
            }
            return Result.Failure($"The collection with an ID of {collectionId} does not exist.");
        }

        private async Task<UserCollections> GetOrCreateUserCollectionsAsync()
        {
            var parsedJSON = await _localStorage.GetItemAsync<UserCollections>("UserCollections");
            if (parsedJSON == null)
            {
                UserCollections newCollectionsObject = new()
                {
                    Collections = new List<Collection>()
                };
                await _localStorage.SetItemAsync<UserCollections>("UserCollections", newCollectionsObject);
                return newCollectionsObject;
            }
            return parsedJSON;
        }
    }
}
