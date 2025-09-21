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
        public async Task<Shared.Result<PaginatedResponse<ArtworkPreview>>> GetArtworkPreviewsAsync(int count, int resultsPerPage, int pageNum)
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

            var processedArtworks = allArtworks;

            if (filters != null && filters.Any())
            {
                var filteredResult = Filter(processedArtworks, filters);
                if (!filteredResult.IsSuccess)
                {
                    return Shared.Result<PaginatedResponse<ArtworkPreview>>.Failure(filteredResult.Message, filteredResult.StatusCode);
                }
                processedArtworks = filteredResult.Value;
            }

            if (!string.IsNullOrEmpty(sort))
            {
                var sortResult = Sort(processedArtworks, sort);
                if (!sortResult.IsSuccess)
                {
                    return Shared.Result<PaginatedResponse<ArtworkPreview>>.Failure(sortResult.Message, sortResult.StatusCode);
                }
                processedArtworks = sortResult.Value;
            }

            var paginatedResponse = PaginatedResponseBuilder<ArtworkPreview>.Build(processedArtworks, resultsPerPage, pageNum);

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
                "alphabetically" => isAscending ? list.OrderBy(a => a.Title).ToList() : list.OrderByDescending(a => a.Title).ToList(),
                "date" => isAscending ? list.OrderBy(a => a.SortableYear).ToList() : list.OrderByDescending(a => a.SortableYear).ToList()
            };

            return Result<List<ArtworkPreview>>.Success(sortedList);

        }

        protected Result<List<ArtworkPreview>> Filter(List<ArtworkPreview> list, List<string> options)
        {
            Result<Dictionary<string, List<string>>> parseOptionsResult = ParseAndValidateFilters(options);
            if (!parseOptionsResult.IsSuccess)
                return Result<List<ArtworkPreview>>.Failure(parseOptionsResult.Message, parseOptionsResult.StatusCode);

            var query = list.AsQueryable();

            foreach (KeyValuePair<string, List<string>> pair in parseOptionsResult.Value)
            {
                switch (pair.Key.ToLower())
                {
                    case "artist":
                        query = query.Where(art => art.Artists != null &&
                            art.Artists.Any(artist => artist != null && artist.Name != null &&
                                pair.Value.Any(keyword => artist.Name.ToLowerInvariant().Contains(keyword.ToLowerInvariant()))));
                        break;

                    case "subject":
                        query = query.Where(art => art.Subjects != null &&
                            art.Subjects.Any(subject => subject != null &&
                            pair.Value.Any(keyword => subject.ToLowerInvariant().Contains(keyword.ToLowerInvariant()))));
                        break;

                    case "type":
                        var parsedTypes = new List<ArtworkType>();
                        foreach (string type in pair.Value)
                        {
                            if (Enum.TryParse(type, true, out ArtworkType parsedType))
                            {
                                parsedTypes.Add(parsedType);
                            }
                        }
                        if (parsedTypes.Any())
                        {
                            query = query.Where(art => parsedTypes.Contains(art.Type));
                        }
                        break;

                    case "material":
                        query = query.Where(art => art.Materials != null &&
                            art.Materials.Any(material => material != null &&
                            pair.Value.Any(keyword => material.ToLowerInvariant().Contains(keyword.ToLowerInvariant()))));
                        break;

                    case "date":
                        foreach (string dateFilter in pair.Value)
                        {
                            if (dateFilter.Contains('-'))
                            {
                                var parts = dateFilter.Split('-');
                                if (parts.Length == 2 &&
                                    int.TryParse(parts[0], out int startYear) &&
                                    int.TryParse(parts[1], out int endYear))
                                {
                                    query = query.Where(art =>
                                        (art.DateStart.HasValue && art.DateEnd.HasValue &&
                                         art.DateStart.Value <= endYear && art.DateEnd.Value >= startYear) ||
                                        (art.DateStart.HasValue && !art.DateEnd.HasValue &&
                                         art.DateStart.Value >= startYear && art.DateStart.Value <= endYear) ||
                                        (art.SortableYear.HasValue &&
                                         art.SortableYear.Value >= startYear && art.SortableYear.Value <= endYear));
                                }
                            }
                            else
                            {
                                if (int.TryParse(dateFilter, out int year))
                                {
                                    query = query.Where(art =>
                                        (art.DateStart.HasValue && art.DateEnd.HasValue &&
                                         year >= art.DateStart.Value && year <= art.DateEnd.Value) ||
                                        (art.DateStart.HasValue && !art.DateEnd.HasValue &&
                                         art.DateStart.Value == year) ||
                                        (art.SortableYear.HasValue && art.SortableYear.Value == year));
                                }
                            }
                        }
                        break;

                    default:
                        break;
                }
            }

            var filteredList = query.ToList();
            return Result<List<ArtworkPreview>>.Success(filteredList);
        }

        protected Result<(string sortby, char order)> ParseAndValidateSortQuery(string sortQuery)
        {
            var supportedSortFields = new HashSet<string> { "alphabetically", "date" };

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

        private bool IsValidDateRange(string dateValue)
        {
            if (dateValue.Length == 4 && int.TryParse(dateValue, out int singleYear))
            {
                return singleYear > 0 && singleYear <= DateTime.Now.Year + 10;
            }

            if (dateValue.Contains('-'))
            {
                var parts = dateValue.Split('-');
                if (parts.Length == 2 &&
                    int.TryParse(parts[0], out int startYear) &&
                    int.TryParse(parts[1], out int endYear))
                {
                    return startYear > 0 && endYear > 0 && startYear <= endYear &&
                           endYear <= DateTime.Now.Year + 10;
                }
            }

            return false;
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
                var filterValue = filterKeyAndValue[1].Trim();

                if (supportedFields.Contains(filterKey))
                {
                    if (!filters.ContainsKey(filterKey))
                    {
                        filters[filterKey] = new List<string>();
                    }

                    if (filterKey == "date")
                    {
                        if (IsValidDateRange(filterValue))
                        {
                            filters[filterKey].Add(filterValue);
                        }
                        else
                        {
                            warnings.Add($"Invalid date format: '{filterValue}'. Expected format: 'YYYY-YYYY' (e.g., '1990-2020') or single year 'YYYY'.");
                            continue;
                        }
                    }
                    else
                    {
                        filters[filterKey].Add(filterValue.ToLowerInvariant());
                    }
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
                return Result<Dictionary<string, List<string>>>.Failure($"No valid filters were found. {string.Join(". ", warnings)}", System.Net.HttpStatusCode.BadRequest);
            }
        }

        public async Task<Result<Artwork>> GetArtworkByIdAsync(int id, ArtworkSource source)
        {
            return await _artworksRepository.GetArtworkById(id, source);
        }
    }
}

