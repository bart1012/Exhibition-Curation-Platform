using ECP.Shared;
using System.Text.Json.Serialization;

namespace ECP.API.Features.Artworks.Clients.ChicagoArtInstitute.Models
{
    public class ChicagoArtwork
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("artist_titles")]
        public List<string>? ArtistTitles { get; set; }

        [JsonPropertyName("thumbnail")]
        public Thumbnail? Thumbnail { get; set; }

        [JsonPropertyName("image_id")]
        public string? ImageId { get; set; }

        [JsonPropertyName("date_display")]
        public string? DateDisplay { get; set; }

        [JsonPropertyName("date_start")]
        public int? DateStart { get; set; }

        [JsonPropertyName("date_end")]
        public int? DateEnd { get; set; }

        [JsonPropertyName("artwork_type_title")]
        public string? ArtworkTypeTitle { get; set; }

        [JsonPropertyName("classification_titles")]
        public List<string>? ClassificationTitles { get; set; }

        [JsonPropertyName("category_titles")]
        public List<string>? CategoryTitles { get; set; }

        [JsonPropertyName("material_titles")]
        public List<string>? MaterialTitles { get; set; }

        [JsonPropertyName("medium_display")]
        public string? MediumDisplay { get; set; }

        [JsonPropertyName("technique_titles")]
        public List<string>? TechniqueTitles { get; set; }

        [JsonPropertyName("subject_titles")]
        public List<string>? SubjectTitles { get; set; }

        [JsonPropertyName("style_titles")]
        public List<string>? StyleTitles { get; set; }

        [JsonPropertyName("place_of_origin")]
        public string? PlaceOfOrigin { get; set; }

        //full artwork properties

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("dimensions")]
        public string? DimensionsDisplay { get; set; }

        [JsonPropertyName("dimensions_detail")]
        public List<Dimensions?>? Dimensions { get; set; }

        [JsonPropertyName("department_title")]
        public string? Department { get; set; }

        [JsonPropertyName("gallery_title")]
        public string? Gallery { get; set; }

        [JsonPropertyName("term_titles")]
        public List<string>? Terms { get; set; }

        [JsonPropertyName("theme_titles")]
        public List<string>? Themes { get; set; }

        [JsonPropertyName("api_link")]
        public string? SourceUrl { get; set; }
    }
}
