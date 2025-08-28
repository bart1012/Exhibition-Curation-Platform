using System.Text.Json.Serialization;

namespace ECP.API.Features.Artworks.Clients.ClevelandMuseum.Models
{
    public class ClevelandMuseumDimensions
    {
        [JsonPropertyName("framed")]
        public FrameMeasurements Framed { get; set; }

        [JsonPropertyName("unframed")]
        public UnframedMeasurements Unframed { get; set; }
    }

    public class FrameMeasurements
    {
        [JsonPropertyName("height")]
        public double Height { get; set; }

        [JsonPropertyName("width")]
        public double Width { get; set; }

        [JsonPropertyName("depth")]
        public double Depth { get; set; }
    }

    public class UnframedMeasurements
    {
        [JsonPropertyName("height")]
        public double Height { get; set; }

        [JsonPropertyName("width")]
        public double Width { get; set; }

    }
}
