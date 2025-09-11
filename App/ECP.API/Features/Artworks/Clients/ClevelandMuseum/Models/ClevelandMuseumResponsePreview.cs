using System.Text.Json.Serialization;

namespace ECP.API.Features.Artworks.Clients.ClevelandMuseum.Models
{
    public class ClevelandMuseumResponsePreview
    {
        [JsonPropertyName("info")]
        public ResponseInfo Info { get; set; }

        [JsonPropertyName("data")]
        public List<ClevelandArtworkPreview> Data { get; set; }
    }

}
