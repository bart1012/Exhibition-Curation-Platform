namespace ECP.Shared
{
    public class ArtworkFilters
    {
        public string Artist { get; set; } = string.Empty;
        public int? DateFrom { get; set; }
        public int? DateTo { get; set; }
        public string Date => DateFrom.HasValue || DateTo.HasValue ?
        $"{DateFrom?.ToString() ?? ""} - {DateTo?.ToString() ?? ""}" : string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Material { get; set; } = string.Empty;
    }
}
