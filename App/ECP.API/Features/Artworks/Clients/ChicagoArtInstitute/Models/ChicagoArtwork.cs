using System.Text.Json.Serialization;

namespace ECP.API.Features.Artworks.Clients.ChicagoArtInstitute.Models
{
    public class ChicagoArtwork
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("artist_titles")]
        public List<string> Artists { get; set; }

        [JsonPropertyName("artwork_type_title")]
        public string Type { get; set; }

        [JsonPropertyName("date_display")]
        public string? DateDisplay { get; set; }

        [JsonPropertyName("date_start")]
        public int? EarliestCreationDate { get; set; }

        [JsonPropertyName("date_end")]
        public int? LatestCreationDate { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("thumbnail")]
        public Thumbnail ThumbnailImage { get; set; }

        [JsonPropertyName("image_id")]
        public string ImageId { get; set; }

        [JsonPropertyName("place_of_origin")]
        public string PlaceOfOrigin { get; set; }

        [JsonPropertyName("dimensions")]
        public string Dimensions { get; set; }

        [JsonPropertyName("style_titles")]
        public List<string> Styles { get; set; }

        [JsonPropertyName("medium_display")]
        public string MediumDisplay { get; set; }


    }
}
