using System.Text.Json.Serialization;

namespace ECP.API.Features.Artworks.Clients.ClevelandMuseum.Models
{
    public class ClevelandArtwork : ClevelandArtworkPreview
    {


        [JsonPropertyName("dimensions")]
        public ClevelandMuseumDimensions Dimensions { get; set; }

        [JsonPropertyName("measurements")]
        public string Measurements { get; set; }

        [JsonPropertyName("state_of_the_work")]
        public string StateOfTheWork { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("current_location")]
        public string CurrentLocation { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("related_works")]
        public List<string> RelatedWorks { get; set; }

        [JsonPropertyName("series")]
        public string Series { get; set; }

        public override string? ToString()
        {
            return $"""
                Id: {Id}
                Title: {Title}
                """;
        }
    }
}
