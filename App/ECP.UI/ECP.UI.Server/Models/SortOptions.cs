namespace ECP.UI.Server.Models
{
    public class SortOptions
    {
        public SortField SortBy { get; set; }
        public bool IsDescending { get; set; }

        public string BuildSortQuery()
        {
            return $"{(IsDescending ? '-' : "%2B")}{SortBy.ToString().ToLowerInvariant()}";
        }
    }
}
