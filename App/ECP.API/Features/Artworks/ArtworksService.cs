// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using ECP.API.Utils;
using ECP.Shared;
using Microsoft.Extensions.Caching.Memory;


namespace ECP.API.Features.Artworks
{
    public interface IArtworksService
    {
        Task<Result<Artwork>> GetArtworkByIdAsync(int id, ArtworkSource source);
        Task<Shared.Result<PaginatedResponse<ArtworkPreview>>> GetArtworkPreviewsAsync(int count, int resultsPerPage, int pageNum);
        Task<Shared.Result<PaginatedResponse<ArtworkPreview>>> SearchAllArtworkPreviewsAsync(string q, string? sort, List<string>? filters, int resultsPerPage, int pageNum);
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

        public async Task<Shared.Result<PaginatedResponse<ArtworkPreview>>> SearchAllArtworkPreviewsAsync(string q, string? sort = null, List<string>? filters = null, int resultsPerPage = 25, int pageNum = 1)
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

            if (filters != null && filters.Any())
            {
                var filteredResult = Filter(allArtworks, filters);

                if (filteredResult.IsSuccess)
                {
                    var filteredArtworks = filteredResult.Value;

                    var paginatedSortedResponse = PaginatedResponseBuilder<ArtworkPreview>.Build(filteredArtworks, resultsPerPage, pageNum);

                    return Shared.Result<PaginatedResponse<ArtworkPreview>>.Success(paginatedSortedResponse);
                }

                return Shared.Result<PaginatedResponse<ArtworkPreview>>.Failure(filteredResult.Message, filteredResult.StatusCode);

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

        protected Result<List<ArtworkPreview>> Sort(List<ArtworkPreview> list, string sortQuery)
        {

            Result<(string sortby, char order)> queryValidation = ParseAndValidateSortQuery(sortQuery);

            if (!queryValidation.IsSuccess)
            {
                return Result<List<ArtworkPreview>>.Failure(queryValidation.Message, queryValidation.StatusCode);
            }

            bool isAscending = queryValidation.Value.order == '+';

            List<ArtworkPreview> sortedList = queryValidation.Value.sortby switch
            {
                "title" => isAscending ? list.OrderBy(a => a.Title).ToList() : list.OrderByDescending(a => a.Title).ToList(),
                "date" => isAscending ? list.OrderBy(a => a.CreationYear).ToList() : list.OrderByDescending(a => a.CreationYear).ToList()
            };

            return Result<List<ArtworkPreview>>.Success(sortedList);

        }

        protected Result<List<ArtworkPreview>> Filter(List<ArtworkPreview> list, List<string> options)
        {
            Result<Dictionary<string, List<string>>> parseOptionsResult = ParseAndValidateFilters(options);
            if (!parseOptionsResult.IsSuccess) return Result<List<ArtworkPreview>>.Failure(parseOptionsResult.Message, parseOptionsResult.StatusCode);
            var query = list.AsQueryable();
            //bool hasSameElements = listA.Intersect(listB).Any();
            foreach (KeyValuePair<string, List<string>> pair in parseOptionsResult.Value)
            {
                switch (pair.Key.ToLower())
                {
                    case "artist":
                        query = query.Where(art => art.Artists.Any(artist =>
                        pair.Value.Any(keyword => artist.Name.ToLowerInvariant().Contains(keyword)))); break;
                    case "subject":
                        query = query.Where(art => art.Subjects.Any(subject =>
                        pair.Value.Any(keyword => subject.ToLowerInvariant().Contains(keyword)))); break;
                    case "type":
                        var parsedTypes = new List<ArtworkType>();
                        foreach (string type in pair.Value)
                        {
                            if (Enum.TryParse(type, true, out ArtworkType parsedType))
                            {
                                parsedTypes.Add(parsedType);
                            }
                        }
                        query = query.Where(art => parsedTypes.Any(type => art.ArtworkType == type));
                        break;
                    case "material":
                        query = query.Where(art => art.Materials.Any(material =>
                        pair.Value.Any(keyword => material.ToLowerInvariant().Contains(keyword)))); break;
                    default: break;
                }
            }

            var filteredList = query.ToList();

            return Result<List<ArtworkPreview>>.Success(filteredList);

        }

        protected Result<(string sortby, char order)> ParseAndValidateSortQuery(string sortQuery)
        {
            var supportedSortFields = new HashSet<string> { "title", "date" };

            if (!string.IsNullOrEmpty(sortQuery))
            {
                if (sortQuery[0] == '+' || sortQuery[0] == '-')
                {
                    char orderChar = sortQuery[0];
                    string field = sortQuery.Substring(1).Trim();
                    bool isAscending = sortQuery[0] == '+';
                    if (supportedSortFields.Contains(field))
                    {
                        return Result<(string sortby, char order)>.Success((field, orderChar));
                    }
                    else
                    {
                        return Shared.Result<(string sortby, char order)>.Failure($"Invalid sort field '{field}'. Supported fields are: {string.Join(", ", supportedSortFields)}", System.Net.HttpStatusCode.BadRequest);

                    }
                }
                else
                {
                    return Shared.Result<(string sortby, char order)>.Failure($"Please specify the sort order by using '-' or '+' before the field name.", System.Net.HttpStatusCode.BadRequest);
                }
            }
            else
            {
                return Shared.Result<(string sortby, char order)>.Failure($"Sort field cannot be empty or null.", System.Net.HttpStatusCode.BadRequest);
            }
        }

        protected Result<Dictionary<string, List<string>>> ParseAndValidateFilters(List<string>? filterQueries)
        {

            if (filterQueries == null || !filterQueries.Any())
            {
                return Result<Dictionary<string, List<string>>>.Failure($"Sort field cannot be empty or null. Please add a valid filter value or remove the parameter.", System.Net.HttpStatusCode.BadRequest);
            }

            var supportedFields = new HashSet<string> { "artist", "date", "subject", "type", "material" };
            var warnings = new List<string>();


            Dictionary<string, List<string>> filters = new();

            foreach (string filterQuery in filterQueries)
            {
                var filterKeyAndValue = filterQuery.Split(':', 2);

                if (filterKeyAndValue.Length != 2)
                {
                    warnings.Add($"Invalid filter format: '{filterQuery}'. Expected 'field:value'.");
                    continue;
                }

                var filterKey = filterKeyAndValue[0].Trim().ToLowerInvariant();
                var filterValue = filterKeyAndValue[1].ToLowerInvariant().Trim();

                if (supportedFields.Contains(filterKey))
                {
                    if (!filters.ContainsKey(filterKey))
                    {
                        filters[filterKey] = new List<string>();
                    }
                    filters[filterKey].Add(filterValue);
                }
                else
                {
                    warnings.Add($"Unsupported field: '{filterKey}'. Supported fields are: {string.Join(", ", supportedFields)}.");
                }
            }

            if (filters.Any())
            {
                return Result<Dictionary<string, List<string>>>.Success(filters, string.Join(". ", warnings));

            }
            else
            {
                return Shared.Result<Dictionary<string, List<string>>>.Failure($"No valid filters were found. {string.Join(". ", warnings)}", System.Net.HttpStatusCode.BadRequest);
            }



        }

        public async Task<Result<Artwork>> GetArtworkByIdAsync(int id, ArtworkSource source)
        {
            return await _artworksRepository.GetArtworkById(id, source);
        }
    }
}

