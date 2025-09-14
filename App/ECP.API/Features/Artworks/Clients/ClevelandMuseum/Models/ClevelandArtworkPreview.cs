using System.Text.Json.Serialization;

namespace ECP.API.Features.Artworks.Clients.ClevelandMuseum.Models
{
    public class ClevelandArtworkPreview
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("creation_date")]
        public string CreationDateDisplay { get; set; }

        [JsonPropertyName("creation_date_earliest")]
        public int? DateStart { get; set; }

        [JsonPropertyName("creation_date_latest")]
        public int? DateEnd { get; set; }

        [JsonPropertyName("sortable_date")]
        public int? SortableYear { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("technique")]
        public string Technique { get; set; }

        [JsonPropertyName("collection")]
        public string Collection { get; set; }

        [JsonPropertyName("culture")]
        public List<string> Cultures { get; set; }

        [JsonPropertyName("department")]
        public string Department { get; set; }

        [JsonPropertyName("creators")]
        public List<ClevelandMuseumArtist> Creators { get; set; }

        [JsonPropertyName("images")]
        public ClevelandMuseumImages Images { get; set; }

        [JsonPropertyName("alternate_images")]
        public List<ClevelandMuseumImages> AltImages { get; set; }

        [JsonPropertyName("support_materials")]
        public List<Material>? Materials { get; set; }

    }

    public class Material
    {
        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}
