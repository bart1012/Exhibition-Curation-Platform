using System.Text.Json.Serialization;

namespace ECP.API.Features.Artworks.Clients.ClevelandMuseum.Models
{
    public class ClevelandMuseumImages
    {
        [JsonPropertyName("web")]
        public ImageData? Web { get; set; }
        [JsonPropertyName("print")]
        public ImageData? Print { get; set; }
        [JsonPropertyName("full")]
        public ImageData? Full { get; set; }
    }

    public class ImageData
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("width")]
        public string Width { get; set; }

        [JsonPropertyName("height")]
        public string Height { get; set; }

        [JsonPropertyName("filesize")]
        public string Filesize { get; set; }

        [JsonPropertyName("filename")]
        public string Filename { get; set; }
    }
}
