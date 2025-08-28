namespace ECP.API.Features.Artworks.Models
{
    public class Artwork
    {
        //Internal Use Properties 
        public string Id { get; set; }
        public ArtworkSource Source { get; set; }
        public int SourceId { get; set; }

        //Display properties
        public string Title { get; set; }
        public List<Artist>? Artists { get; set; }
        public string CreationDate { get; set; }
        public int? MinCreationYear { get; set; }
        public int? MaxCreationYear { get; set; }
        public string Medium { get; set; }
        public string Type { get; set; }
        public List<string>? Culture { get; set; }
        public Dimensions Dimensions { get; set; }
        public string Description { get; set; }
        public string SourceUrl { get; set; }
        public Images? Images { get; set; }

    }

    public class Dimensions
    {
        public double Height { get; set; }
        public double Width { get; set; }
        public double? Depth { get; set; }
    }

    public class Image
    {
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class Images
    {
        public Image? Thumbnail { get; set; }
        public Image Web { get; set; }
        public Image? Full { get; set; }

    }
}
