namespace ECP.Shared
{
    public class ArtworkPreview
    {
        //Internal Use Properties 
        public string Id { get; set; } = string.Empty;
        public ArtworkSource Source { get; set; }
        public int SourceId { get; set; }

        //Basic Information
        public string Title { get; set; }
        public List<Artist>? Artists { get; set; }
        public Image? Thumbnail { get; set; }

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


        public override bool Equals(object? obj)
        {
            if (obj is not ArtworkPreview other)
            {
                return false;
            }

            return Title == other.Title && Artists?[0]?.Name == other.Artists?[0]?.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, Artists?[0]?.Name);
        }
    }
}
