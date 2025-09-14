using ECP.Shared;
using ECP.UI.Client.Services;
using Microsoft.AspNetCore.Components;

namespace ECP.UI.Server.Components.Artworks
{
    public class UserCollectionsBase : ComponentBase
    {
        [Inject]
        protected IUserCollectionsService CollectionsService { get; set; }

        protected List<Collection> _userCollections = new();

        protected async Task FetchCollections()
        {
            try
            {
                var collectionsResult = await CollectionsService.GetCollectionsAsync();
                if (collectionsResult.IsSuccess)
                {
                    _userCollections = collectionsResult.Value.Collections.ToList();
                    Console.WriteLine($"Fetched {_userCollections.Count} collections");
                }
                else
                {
                    Console.WriteLine($"Error fetching collections: {collectionsResult.Message}");
                    _userCollections = new List<Collection>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception fetching collections: {ex.Message}");
                _userCollections = new List<Collection>();
            }

        }
    }
}
