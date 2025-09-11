namespace ECP.Shared
{
    public class Collection
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public List<Artwork> Artworks { get; set; } = new List<Artwork>();
    }
}
