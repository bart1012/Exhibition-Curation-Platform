namespace ECP.API.Features.Artworks.Clients.ChicagoArtInstitute.Models
{
    public class ChicagoApiParameters
    {
        public int Count { get; set; }
        public string? Query { get; set; }
        public int Offset { get; set; }
        public bool PreviewsOnly { get; set; }
        public int Page { get; set; }
    }
}
