using System.Text.Json.Serialization;

namespace ECP.API.Features.Artworks.Clients.ClevelandMuseum.Models
{
    public class ClevelandMuseumArtworkPreview
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("creators")]
        public List<ClevelandMuseumArtist> Creators { get; set; }

        [JsonPropertyName("images")]
        public ClevelandMuseumImages Images { get; set; }

    }
}
