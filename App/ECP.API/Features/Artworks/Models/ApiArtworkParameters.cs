namespace ECP.API.Features.Artworks.Models
{
    public class ApiArtworkParameters
    {
        public int Count { get; set; }
        public string? Query { get; set; }
        public int Offset { get; set; }
        public bool PreviewsOnly { get; set; }
    }
}
