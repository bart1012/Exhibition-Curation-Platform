using System.Text.Json;
using System.Text.Json.Serialization;

namespace ECP.API.Features.Artworks.Clients.ClevelandMuseum.Models
{
    public class ClevelandMuseumResponse
    {
        [JsonPropertyName("info")]
        public ResponseInfo Info { get; set; }

        [JsonPropertyName("data")]
        public List<ClevelandMuseumArtwork> Data { get; set; }
    }

    public class ResponseInfo
    {
        public int Total { get; set; }
        [JsonExtensionData]
        public Dictionary<string, JsonElement> Parameters { get; set; }

    }
}
