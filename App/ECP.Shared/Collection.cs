namespace ECP.Shared
{
    public class Collection(string name)
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = name;
        public List<ArtworkPreview> Artworks { get; set; } = new List<ArtworkPreview>();
    }
}
