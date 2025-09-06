using System.Text.Json.Serialization;

namespace ECP.API.Features.Artworks.Clients.ChicagoArtInstitute.Models
{
    public class ChicagoApiResponse<T>
    {
        public List<T> Data { get; set; }

        [JsonPropertyName("pagination")]
        public ChicagoResponseMetadata Info { get; set; }

        public override string? ToString()
        {
            return $"""
                Total results: {Info.Total}
                Total pages: {Info.Pages}
                Current results count: {Data.Count()}
                """;
        }
    }

    public class ChicagoResponseMetadata
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("total_pages")]
        public int Pages { get; set; }

    }
}
