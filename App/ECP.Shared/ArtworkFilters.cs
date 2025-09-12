namespace ECP.Shared
{
    public class ArtworkFilters
    {
        public string Artist { get; set; } = string.Empty;
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Date => DateFrom.HasValue || DateTo.HasValue ?
            $"{DateFrom?.ToString("yyyy-MM-dd") ?? ""} - {DateTo?.ToString("yyyy-MM-dd") ?? ""}" : string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Material { get; set; } = string.Empty;
    }
}
