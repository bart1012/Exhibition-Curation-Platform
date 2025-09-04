namespace ECP.Shared
{
    public class ArtworkPreview
    {
        //Internal Use Properties 
        public string Id { get; set; }
        public ArtworkSource Source { get; set; }
        public int SourceId { get; set; }

        //Display properties
        public string Title { get; set; }
        public List<Artist>? Artists { get; set; }
        public Image? WebImage { get; set; }
    }
}
