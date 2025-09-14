using Blazored.LocalStorage;
using ECP.Shared;

namespace ECP.UI.Client.Services
{
    public interface IUserCollectionsService
    {
        Task<Result<UserCollections>> GetCollectionsAsync();
        Task<Result> CreateCollectionAsync(Collection collection);
        Task<Result> AddArtworkToCollectionAsync(string collectionId, ArtworkPreview artwork);
        Task<Result> RemoveArtworkFromCollectionAsync(string collectionId, string artworkId);
        Task<Result<List<ArtworkPreview>>> GetCollectionArtworksAsync(string collectionId);
        Task<Result<Collection>> GetCollectionAsync(string collectionId);
        Task<bool> CollectionsExistAsync();
        Task<Result> CreateUserCollectionsObjectAsync();
    }

    public class UserCollectionsService(ILocalStorageService localStorage) : IUserCollectionsService
    {
        private readonly ILocalStorageService _localStorage = localStorage;
        private const string COLLECTIONS_KEY = "UserCollections";

        public async Task<bool> CollectionsExistAsync()
        {
            try
            {
                var collections = await _localStorage.GetItemAsync<UserCollections>(COLLECTIONS_KEY);
                return collections != null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error checking if collections exist: {e.Message}");
                return false;
            }
        }

        public async Task<Result> CreateUserCollectionsObjectAsync()
        {
            try
            {
                var newCollectionsObject = new UserCollections
                {
                    Collections = new List<Collection>()
                };

                await _localStorage.SetItemAsync<UserCollections>(COLLECTIONS_KEY, newCollectionsObject);
                return Result.Success();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error creating user collections object: {e.Message}");
                return Result.Failure($"Failed to create collections object: {e.Message}");
            }
        }

        public async Task<Result> AddArtworkToCollectionAsync(string collectionId, ArtworkPreview artwork)
        {
            try
            {
                var userCollections = await EnsureUserCollectionsExistAsync();
                var collection = userCollections.Collections.FirstOrDefault(c => c.Id == collectionId);

                if (collection == null)
                {
                    return Result.Failure($"The collection with an ID of {collectionId} does not exist.");
                }

                bool alreadyExists = collection.Artworks.Any(a => a.Id == artwork.Id);
                if (alreadyExists)
                {
                    return Result.Failure($"An artwork with an ID of {artwork.Id} already exists in '{collection.Name}'");
                }

                collection.Artworks.Add(artwork);
                await _localStorage.SetItemAsync<UserCollections>(COLLECTIONS_KEY, userCollections);
                return Result.Success();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error adding artwork to collection: {e.Message}");
                return Result.Failure($"Failed to add artwork to collection: {e.Message}");
            }
        }

        public async Task<Result> CreateCollectionAsync(Collection collection)
        {
            try
            {
                var userCollections = await EnsureUserCollectionsExistAsync();

                // Check if collection with same ID already exists
                if (userCollections.Collections.Any(c => c.Id == collection.Id))
                {
                    return Result.Failure($"A collection with ID '{collection.Id}' already exists.");
                }

                userCollections.Collections.Add(collection);
                await _localStorage.SetItemAsync<UserCollections>(COLLECTIONS_KEY, userCollections);
                return Result.Success();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error creating collection: {e.Message}");
                return Result.Failure($"Failed to create collection: {e.Message}");
            }
        }

        public async Task<Result<List<ArtworkPreview>>> GetCollectionArtworksAsync(string collectionId)
        {
            try
            {
                var userCollections = await EnsureUserCollectionsExistAsync();
                var collection = userCollections.Collections.FirstOrDefault(c => c.Id == collectionId);

                if (collection == null)
                {
                    return Result<List<ArtworkPreview>>.Failure($"Collection with ID '{collectionId}' not found.");
                }

                return Result<List<ArtworkPreview>>.Success(collection.Artworks);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting collection artworks: {e.Message}");
                return Result<List<ArtworkPreview>>.Failure($"Failed to get collection artworks: {e.Message}");
            }
        }

        public async Task<Result<UserCollections>> GetCollectionsAsync()
        {
            try
            {
                var collections = await _localStorage.GetItemAsync<UserCollections>(COLLECTIONS_KEY);

                if (collections == null)
                {
                    // Try to create the collections object first
                    var createResult = await CreateUserCollectionsObjectAsync();
                    if (!createResult.IsSuccess)
                    {
                        return Result<UserCollections>.Failure("No collections exist and failed to create collections object.");
                    }

                    // Return the newly created empty collections object
                    collections = new UserCollections { Collections = new List<Collection>() };
                }

                return Result<UserCollections>.Success(collections);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting collections: {e.Message}");
                return Result<UserCollections>.Failure($"Failed to get collections: {e.Message}");
            }
        }

        public async Task<Result> RemoveArtworkFromCollectionAsync(string collectionId, string artworkId)
        {
            try
            {
                var userCollections = await EnsureUserCollectionsExistAsync();
                var collection = userCollections.Collections.FirstOrDefault(c => c.Id == collectionId);

                if (collection == null)
                {
                    return Result.Failure($"The collection with an ID of {collectionId} does not exist.");
                }

                int numRemoved = collection.Artworks.RemoveAll(a => string.Equals(a.Id, artworkId));

                if (numRemoved == 0)
                {
                    return Result.Failure($"Could not find an artwork with ID: {artworkId}.");
                }

                await _localStorage.SetItemAsync<UserCollections>(COLLECTIONS_KEY, userCollections);
                return Result.Success();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error removing artwork from collection: {e.Message}");
                return Result.Failure($"Failed to remove artwork from collection: {e.Message}");
            }
        }

        private async Task<UserCollections> EnsureUserCollectionsExistAsync()
        {
            var collections = await _localStorage.GetItemAsync<UserCollections>(COLLECTIONS_KEY);

            if (collections == null)
            {
                var newCollectionsObject = new UserCollections
                {
                    Collections = new List<Collection>()
                };

                await _localStorage.SetItemAsync<UserCollections>(COLLECTIONS_KEY, newCollectionsObject);
                return newCollectionsObject;
            }

            return collections;
        }

        public async Task<Result<Collection>> GetCollectionAsync(string collectionId)
        {
            try
            {
                var userCollections = await EnsureUserCollectionsExistAsync();
                var collection = userCollections.Collections.FirstOrDefault(c => c.Id == collectionId);

                if (collection == null)
                {
                    return Result<Collection>.Failure($"Collection with ID '{collectionId}' not found.");
                }

                return Result<Collection>.Success(collection);
            }
            catch (Exception e)
            {
                // Corrected message to reflect what the method is doing.
                Console.WriteLine($"Error getting collection: {e.Message}");
                return Result<Collection>.Failure($"Failed to get collection: {e.Message}");
            }
        }
    }
}