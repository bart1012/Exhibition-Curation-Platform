using System.Text.Json.Serialization;

namespace ECP.API.Features.Artworks.Clients.ClevelandMuseum.Models
{
    public class ClevelandArtwork
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("creation_date")]
        public string CreationDate { get; set; }

        [JsonPropertyName("creation_date_earliest")]
        public int CreationDateEarliest { get; set; }

        [JsonPropertyName("creation_date_latest")]
        public int CreationDateLatest { get; set; }

        [JsonPropertyName("technique")]
        public string Technique { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("measurements")]
        public string Measurements { get; set; }

        [JsonPropertyName("culture")]
        public List<string> Culture { get; set; }

        [JsonPropertyName("dimensions")]
        public ClevelandMuseumDimensions Dimensions { get; set; }

        [JsonPropertyName("creators")]
        public List<ClevelandMuseumArtist> Creators { get; set; }

        [JsonPropertyName("images")]
        public ClevelandMuseumImages Images { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        public override string? ToString()
        {
            return $"""
                Id: {Id}
                Title: {Title}
                """;
        }
    }
}
