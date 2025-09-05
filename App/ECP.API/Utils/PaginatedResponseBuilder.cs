using ECP.Shared;

namespace ECP.API.Utils
{
    public class PaginatedResponseBuilder<T>
    {
        public static PaginatedResponse<T> Build(IEnumerable<T> data, int resultsPerPage, int pageNum)
        {
            var totalCount = data.Count();
            var paginationInfo = new PaginationInfo
            {
                TotalItems = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)resultsPerPage),
                CurrentPage = pageNum,
                ItemsPerPage = resultsPerPage
            };
            var pagedData = data.Skip((pageNum - 1) * resultsPerPage).Take(resultsPerPage).ToList();
            var paginatedResponse = new PaginatedResponse<T>
            {
                Data = pagedData,
                Info = paginationInfo
            };
            return paginatedResponse;
        }
    }
}
