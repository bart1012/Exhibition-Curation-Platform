namespace ECP.UI.Server.Models
{
    public class SortOptions
    {
        public string SortBy { get; set; }
        public bool IsDescending { get; set; }

        public string BuildSortQuery()
        {
            if (string.IsNullOrEmpty(SortBy)) return string.Empty;
            return $"&sort={(IsDescending ? '-' : "%2B")}{SortBy}";
        }
    }
}
