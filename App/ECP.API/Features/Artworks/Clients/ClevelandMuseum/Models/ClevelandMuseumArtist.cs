namespace ECP.API.Features.Artworks.Clients.ClevelandMuseum.Models
{
    public class ClevelandMuseumArtist
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Birth_Year { get; set; }
        public string Death_Year { get; set; }
        public string Role { get; set; }
        public string? Biography { get; set; }
    }
}
