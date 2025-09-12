using ECP.Shared;

namespace ECP.UI.Server.Services
{
    public class ArtworkSeeder
    {
        public static List<ArtworkPreview> SeedPreviews()
        {
            List<ArtworkPreview> artworkPreviews = new();

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CHICAGO_ART_INSTITUTE,
                SourceId = 101,
                ArtworkType = ArtworkType.Painting,
                Title = "The Starry Night",
                Artists = new List<Artist> { new Artist { Name = "Vincent van Gogh" } },
                CreationYear = 1889,
                ArtworkTypeDisplay = "Oil on canvas",
                Materials = new List<string> { "Oil paint", "Canvas" },
                WebImage = new Image { Url = "https://placehold.co/400x300/e9d5ff/6b21a8?text=Van+Gogh" },
                Subjects = new List<string> { "Landscape", "Night Sky" }
            });

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CHICAGO_ART_INSTITUTE,
                SourceId = 202,
                ArtworkType = ArtworkType.Sculpture,
                Title = "The Thinker",
                Artists = new List<Artist> { new Artist { Name = "Auguste Rodin" } },
                CreationYear = 1904,
                ArtworkTypeDisplay = "Bronze sculpture",
                Materials = new List<string> { "Bronze" },
                WebImage = new Image { Url = "https://placehold.co/300x500/cbd5e1/1e293b?text=Rodin" },
                Subjects = new List<string> { "Human Figure", "Philosophy" }
            });

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CLEVELAND_MUSEUM,
                SourceId = 103,
                ArtworkType = ArtworkType.Digital,
                Title = "Migrant Mother",
                Artists = new List<Artist> { new Artist { Name = "Dorothea Lange" } },
                CreationYear = 1936,
                ArtworkTypeDisplay = "Black-and-white photograph",
                Materials = new List<string> { "Silver gelatin print" },
                WebImage = new Image { Url = "https://placehold.co/500x400/f3f4f6/1f2937?text=Lange" },
                Subjects = new List<string> { "Portrait", "Documentary" }
            });

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CLEVELAND_MUSEUM,
                SourceId = 104,
                ArtworkType = ArtworkType.Painting,
                Title = "Girl with a Pearl Earring",
                Artists = new List<Artist> { new Artist { Name = "Johannes Vermeer" } },
                CreationYear = 1665,
                ArtworkTypeDisplay = "Oil on canvas",
                Materials = new List<string> { "Oil paint", "Canvas" },
                WebImage = new Image { Url = "https://placehold.co/300x400/94a3b8/0f172a?text=Vermeer" },
                Subjects = new List<string> { "Portrait" }
            });

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CLEVELAND_MUSEUM,
                SourceId = 205,
                ArtworkType = ArtworkType.Drawing,
                Title = "Vitruvian Man",
                Artists = new List<Artist> { new Artist { Name = "Leonardo da Vinci" } },
                CreationYear = 1490,
                ArtworkTypeDisplay = "Pen and ink on paper",
                Materials = new List<string> { "Paper", "Ink" },
                WebImage = new Image { Url = "https://placehold.co/400x400/a5f3fc/0e7490?text=Da+Vinci" },
                Subjects = new List<string> { "Human Anatomy", "Ideal Proportions" }
            });

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CHICAGO_ART_INSTITUTE,
                SourceId = 106,
                ArtworkType = ArtworkType.Painting,
                Title = "Impression, soleil levant",
                Artists = new List<Artist> { new Artist { Name = "Claude Monet" } },
                CreationYear = 1872,
                ArtworkTypeDisplay = "Oil on canvas",
                Materials = new List<string> { "Oil paint", "Canvas" },
                WebImage = new Image { Url = "https://placehold.co/500x300/fecaca/991b1b?text=Monet" },
                Subjects = new List<string> { "Landscape", "Harbor" }
            });

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CHICAGO_ART_INSTITUTE,
                SourceId = 207,
                ArtworkType = ArtworkType.Sculpture,
                Title = "David",
                Artists = new List<Artist> { new Artist { Name = "Michelangelo" } },
                CreationYear = 1504,
                ArtworkTypeDisplay = "Marble sculpture",
                Materials = new List<string> { "Marble" },
                WebImage = new Image { Url = "https://placehold.co/300x500/94a3b8/1f2937?text=Michelangelo" },
                Subjects = new List<string> { "Human Figure", "Biblical" }
            });

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CHICAGO_ART_INSTITUTE,
                SourceId = 108,
                ArtworkType = ArtworkType.Painting,
                Title = "Mona Lisa",
                Artists = new List<Artist> { new Artist { Name = "Leonardo da Vinci" } },
                CreationYear = 1503,
                ArtworkTypeDisplay = "Oil on poplar panel",
                Materials = new List<string> { "Oil paint", "Wood" },
                WebImage = new Image { Url = "https://placehold.co/400x500/92400e/f9fafb?text=Mona+Lisa" },
                Subjects = new List<string> { "Portrait" }
            });

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CHICAGO_ART_INSTITUTE,
                SourceId = 209,
                ArtworkType = ArtworkType.Painting,
                Title = "The Scream",
                Artists = new List<Artist> { new Artist { Name = "Edvard Munch" } },
                CreationYear = 1893,
                ArtworkTypeDisplay = "Oil, tempera, and pastel on cardboard",
                Materials = new List<string> { "Oil paint", "Cardboard" },
                WebImage = new Image { Url = "https://placehold.co/400x500/fca5a5/450a0a?text=The+Scream" },
                Subjects = new List<string> { "Emotional State", "Anxiety" }
            });

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CLEVELAND_MUSEUM,
                SourceId = 110,
                ArtworkType = ArtworkType.Print,
                Title = "The Great Wave off Kanagawa",
                Artists = new List<Artist> { new Artist { Name = "Katsushika Hokusai" } },
                CreationYear = 1831,
                ArtworkTypeDisplay = "Woodblock print",
                Materials = new List<string> { "Wood", "Paper" },
                WebImage = new Image { Url = "https://placehold.co/500x300/67e8f9/0e7490?text=Hokusai" },
                Subjects = new List<string> { "Landscape", "Nature" }
            });

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CLEVELAND_MUSEUM,
                SourceId = 111,
                ArtworkType = ArtworkType.Painting,
                Title = "The Last Supper",
                Artists = new List<Artist> { new Artist { Name = "Leonardo da Vinci" } },
                CreationYear = 1498,
                ArtworkTypeDisplay = "Tempera on gesso, pitch, and mastic",
                Materials = new List<string> { "Tempera paint", "Gesso", "Plaster" },
                WebImage = new Image { Url = "https://placehold.co/500x300/bfdbfe/1e40af?text=Da+Vinci" },
                Subjects = new List<string> { "Biblical", "Religious" }
            });

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CLEVELAND_MUSEUM,
                SourceId = 212,
                ArtworkType = ArtworkType.Painting,
                Title = "Guernica",
                Artists = new List<Artist> { new Artist { Name = "Pablo Picasso" } },
                CreationYear = 1937,
                ArtworkTypeDisplay = "Oil on canvas",
                Materials = new List<string> { "Oil paint", "Canvas" },
                WebImage = new Image { Url = "https://placehold.co/500x300/e7e5e4/44403c?text=Picasso" },
                Subjects = new List<string> { "War", "Politics" }
            });

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CHICAGO_ART_INSTITUTE,
                SourceId = 113,
                ArtworkType = ArtworkType.Painting,
                Title = "American Gothic",
                Artists = new List<Artist> { new Artist { Name = "Grant Wood" } },
                CreationYear = 1930,
                ArtworkTypeDisplay = "Oil on beaverboard",
                Materials = new List<string> { "Oil paint", "Wood" },
                WebImage = new Image { Url = "https://placehold.co/300x400/e0f2f1/042f2e?text=Wood" },
                Subjects = new List<string> { "Portrait", "Rural Life" }
            });

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CHICAGO_ART_INSTITUTE,
                SourceId = 214,
                ArtworkType = ArtworkType.Painting,
                Title = "The Persistence of Memory",
                Artists = new List<Artist> { new Artist { Name = "Salvador Dalí" } },
                CreationYear = 1931,
                ArtworkTypeDisplay = "Oil on canvas",
                Materials = new List<string> { "Oil paint", "Canvas" },
                WebImage = new Image { Url = "https://placehold.co/400x300/fee2e2/991b1b?text=Dalí" },
                Subjects = new List<string> { "Surrealism", "Time" }
            });



            return artworkPreviews;
        }
    }
}
