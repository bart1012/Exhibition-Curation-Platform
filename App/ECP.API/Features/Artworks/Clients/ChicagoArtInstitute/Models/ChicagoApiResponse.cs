using System.Text.Json.Serialization;

namespace ECP.API.Features.Artworks.Clients.ChicagoArtInstitute.Models
{
    public class ChicagoApiResponse<T>
    {
        public List<T> Data { get; set; }

        public ChicagoResponseMetadata Info { get; set; }
    }

    public class ChicagoResponseMetadata
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("total_pages")]
        public int Pages { get; set; }

    }
}
