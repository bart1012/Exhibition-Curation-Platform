// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using ECP.API.Utils;
using ECP.Shared;
using Microsoft.Extensions.Caching.Memory;


namespace ECP.API.Features.Artworks
{
    public interface IArtworksService
    {
        Task<Shared.Result<PaginatedResponse<ArtworkPreview>>> GetArtworkPreviewsAsync(int count, int resultsPerPage, int pageNum);
        Task<Shared.Result<PaginatedResponse<ArtworkPreview>>> SearchAllArtworkPreviewsAsync(string q, string? sort, int resultsPerPage, int pageNum);
    }
    public class ArtworksService(IArtworksRepository artworksRepository, IMemoryCache memoryCache) : IArtworksService
    {
        private readonly IArtworksRepository _artworksRepository = artworksRepository;
        private readonly IMemoryCache _cache = memoryCache;
        public async Task<Shared.Result<PaginatedResponse<ArtworkPreview>>> GetArtworkPreviewsAsync(int count, int resultsPerPage = 25, int pageNum = 1)
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

        public async Task<Shared.Result<PaginatedResponse<ArtworkPreview>>> SearchAllArtworkPreviewsAsync(string q, string? sort = null, int resultsPerPage = 25, int pageNum = 1)
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
                    var deduplicatedList = DeDuplicate(repositoryResponse.Value);
                    MemoryCacheEntryOptions options = new()
                    {
                        Size = deduplicatedList.Count,
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
                    };
                    _cache.Set(q, deduplicatedList, options);
                    artworks = deduplicatedList;
                }

            }


            var allArtworks = artworks ?? new List<ArtworkPreview>();

            if (!string.IsNullOrEmpty(sort))
            {
                var sortResult = Sort(allArtworks, sort);

                if (sortResult.IsSuccess)
                {
                    var sortedArtworks = sortResult.Value;

                    var paginatedSortedResponse = PaginatedResponseBuilder<ArtworkPreview>.Build(sortedArtworks, resultsPerPage, pageNum);

                    return Shared.Result<PaginatedResponse<ArtworkPreview>>.Success(paginatedSortedResponse);
                }

                return Shared.Result<PaginatedResponse<ArtworkPreview>>.Failure(sortResult.Message, sortResult.StatusCode);

            }

            var paginatedResponse = PaginatedResponseBuilder<ArtworkPreview>.Build(allArtworks, resultsPerPage, pageNum);

            return Shared.Result<PaginatedResponse<ArtworkPreview>>.Success(paginatedResponse);

        }

        protected List<ArtworkPreview> DeDuplicate(IEnumerable<ArtworkPreview> list)
        {
            Console.WriteLine($"Total count (duplicated): {list.Count()}");

            HashSet<ArtworkPreview> hashSet = new();

            foreach (var artwork in list)
            {
                hashSet.Add(artwork);
            }

            var result = hashSet.ToList<ArtworkPreview>();
            Console.WriteLine($"Total count (deduplicated): {result.Count()}");
            return result;

        }

        protected Result<List<ArtworkPreview>> Sort(List<ArtworkPreview> list, string fieldName)
        {
            var supportedSortFields = new HashSet<string> { "title", "date" };

            if (!string.IsNullOrEmpty(fieldName))
            {
                if (fieldName[0] == '+' || fieldName[0] == '-')
                {
                    string field = fieldName.Substring(1).Trim();
                    bool isAscending = fieldName[0] == '+';
                    if (supportedSortFields.Contains(field))
                    {
                        List<ArtworkPreview> sortedList = field switch
                        {
                            "title" => isAscending ? list.OrderBy(a => a.Title).ToList() : list.OrderByDescending(a => a.Title).ToList(),
                            "date" => isAscending ? list.OrderBy(a => a.CreationYear).ToList() : list.OrderByDescending(a => a.CreationYear).ToList()
                        };

                        return Result<List<ArtworkPreview>>.Success(sortedList);
                    }
                    else
                    {
                        return Shared.Result<List<ArtworkPreview>>.Failure($"Invalid sort field '{field}'. Supported fields are: {string.Join(", ", supportedSortFields)}", System.Net.HttpStatusCode.BadRequest);

                    }
                }
                else
                {
                    return Shared.Result<List<ArtworkPreview>>.Failure($"Please specify the sort order by using '-' or '+' before the field name.", System.Net.HttpStatusCode.BadRequest);
                }
            }
            else
            {
                return Shared.Result<List<ArtworkPreview>>.Failure($"Sort field cannot be empty or null.", System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}
