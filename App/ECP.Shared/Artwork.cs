using System.Text.Json.Serialization;

namespace ECP.Shared
{
    public class Artwork
    {
        //Internal Use Properties 
        public string Id { get; set; } = string.Empty;
        public ArtworkSource Source { get; set; }
        public int SourceId { get; set; }

        //Basic Information
        public string Title { get; set; }
        public List<Artist>? Artists { get; set; }

        //Date properties
        public string? DateDisplay { get; set; }
        public int? DateStart { get; set; }
        public int? DateEnd { get; set; }
        public int? SortableYear { get; set; }

        //Type properties
        public ArtworkType Type { get; set; }
        public string TypeDisplay { get; set; } = string.Empty;
        public List<string> Classifications { get; set; } = new();
        public List<string> Categories { get; set; } = new();

        //Material Properties
        public List<string> Materials { get; set; } = new();
        public string? MediumDisplay { get; set; }
        public List<string> Techniques { get; set; } = new();

        //Subject and style properties
        public List<string> Subjects { get; set; } = new();
        public List<string> Styles { get; set; } = new();

        //Cultural properties
        public List<string> Culture { get; set; } = new();
        public string? PlaceOfOrigin { get; set; }

        //Physical properties
        public Dimensions? Dimensions { get; set; }
        public string? DimensionsDisplay { get; set; }

        //Content and description
        public string? Description { get; set; }
        public string? LongDescription { get; set; }
        public List<string> Keywords { get; set; } = new();

        //Historical context
        public string? Period { get; set; }
        public string? Dynasty { get; set; }
        public string? School { get; set; }
        public string? Movement { get; set; }

        //Location
        public string? CurrentLocation { get; set; }
        public string? Department { get; set; }
        public string? Gallery { get; set; }

        //Digital assets and links
        public Images Images { get; set; } = new();
        public string? SourceUrl { get; set; }
        public List<string> RelatedUrls { get; set; } = new();
        public string? WikipediaUrl { get; set; }

        //Relationships
        public List<string> RelatedArtworkIds { get; set; } = new();
        public string? SeriesTitle { get; set; }
        public int? SeriesNumber { get; set; }

    }

    public class Dimensions
    {
        [JsonPropertyName("height")]
        public double Height { get; set; }

        [JsonPropertyName("width")]
        public double Width { get; set; }

        [JsonPropertyName("depth")]
        public double? Depth { get; set; }
    }

    public class Images
    {
        public Image? Thumbnail { get; set; }
        public Image Web { get; set; }
        public Image? Full { get; set; }

    }
}
