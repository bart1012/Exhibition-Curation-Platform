using System.Text.Json.Serialization;

namespace ECP.API.Features.Artworks.Clients.ChicagoArtInstitute.Models
{
    public class ChicagoArtPreview
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("artist_titles")]
        public List<string> Artists { get; set; }

        [JsonPropertyName("date_start")]
        public int? EarliestCreationDate { get; set; }

        [JsonPropertyName("date_end")]
        public int? LatestCreationDate { get; set; }

        [JsonPropertyName("thumbnail")]
        public Thumbnail ThumbnailImage { get; set; }

        [JsonPropertyName("image_id")]
        public string ImageId { get; set; }
    }

    public class Thumbnail
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string AltText { get; set; }

    }
}