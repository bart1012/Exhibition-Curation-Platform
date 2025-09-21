namespace ECP.Shared
{
    public class PaginatedResponse<T>
    {
        public PaginationInfo Info { get; set; }
        public IEnumerable<T> Data { get; set; }


    }

    public class PaginationInfo
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
    }
}
