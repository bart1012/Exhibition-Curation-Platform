namespace ECP.Shared
{
    public class ArtworkPreview
    {
        //Internal Use Properties 
        public string Id { get; set; }
        public ArtworkSource Source { get; set; }
        public int SourceId { get; set; }
        public ArtworkType ArtworkType { get; set; }

        //Display properties
        public string Title { get; set; }
        public List<Artist>? Artists { get; set; }
        public int? CreationYear { get; set; }
        public string ArtworkTypeDisplay { get; set; }

        public List<string> Materials { get; set; }
        public Image? WebImage { get; set; }
        public List<string> Subjects { get; set; }

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
