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
                Type = ArtworkType.Painting,
                Title = "The Starry Night",
                Artists = new List<Artist> { new Artist { Name = "Vincent van Gogh" } },
                DateDisplay = "1889",
                TypeDisplay = "Oil on canvas",
                Materials = new List<string> { "Oil paint", "Canvas" },
                Thumbnail = new Image { Url = "https://placehold.co/400x300/e9d5ff/6b21a8?text=Van+Gogh" },
                Subjects = new List<string> { "Landscape", "Night Sky" }
            });

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CHICAGO_ART_INSTITUTE,
                SourceId = 202,
                Type = ArtworkType.Sculpture,
                Title = "The Thinker",
                Artists = new List<Artist> { new Artist { Name = "Auguste Rodin" } },
                DateDisplay = "1889",
                TypeDisplay = "Bronze sculpture",
                Materials = new List<string> { "Bronze" },
                Thumbnail = new Image { Url = "https://placehold.co/300x500/cbd5e1/1e293b?text=Rodin" },
                Subjects = new List<string> { "Human Figure", "Philosophy" }
            });

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CLEVELAND_MUSEUM,
                SourceId = 103,
                Type = ArtworkType.Digital,
                Title = "Migrant Mother",
                Artists = new List<Artist> { new Artist { Name = "Dorothea Lange" } },
                DateDisplay = "1889",
                TypeDisplay = "Black-and-white photograph",
                Materials = new List<string> { "Silver gelatin print" },
                Thumbnail = new Image { Url = "https://placehold.co/500x400/f3f4f6/1f2937?text=Lange" },
                Subjects = new List<string> { "Portrait", "Documentary" }
            });

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CLEVELAND_MUSEUM,
                SourceId = 104,
                Type = ArtworkType.Painting,
                Title = "Girl with a Pearl Earring",
                Artists = new List<Artist> { new Artist { Name = "Johannes Vermeer" } },
                DateDisplay = "1889",
                TypeDisplay = "Oil on canvas",
                Materials = new List<string> { "Oil paint", "Canvas" },
                Thumbnail = new Image { Url = "https://placehold.co/300x400/94a3b8/0f172a?text=Vermeer" },
                Subjects = new List<string> { "Portrait" }
            });

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CLEVELAND_MUSEUM,
                SourceId = 205,
                Type = ArtworkType.Drawing,
                Title = "Vitruvian Man",
                Artists = new List<Artist> { new Artist { Name = "Leonardo da Vinci" } },
                DateDisplay = "1889",
                TypeDisplay = "Pen and ink on paper",
                Materials = new List<string> { "Paper", "Ink" },
                Thumbnail = new Image { Url = "https://placehold.co/400x400/a5f3fc/0e7490?text=Da+Vinci" },
                Subjects = new List<string> { "Human Anatomy", "Ideal Proportions" }
            });

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CHICAGO_ART_INSTITUTE,
                SourceId = 106,
                Type = ArtworkType.Painting,
                Title = "Impression, soleil levant",
                Artists = new List<Artist> { new Artist { Name = "Claude Monet" } },
                DateDisplay = "1889",
                TypeDisplay = "Oil on canvas",
                Materials = new List<string> { "Oil paint", "Canvas" },
                Thumbnail = new Image { Url = "https://placehold.co/500x300/fecaca/991b1b?text=Monet" },
                Subjects = new List<string> { "Landscape", "Harbor" }
            });

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CHICAGO_ART_INSTITUTE,
                SourceId = 207,
                Type = ArtworkType.Sculpture,
                Title = "David",
                Artists = new List<Artist> { new Artist { Name = "Michelangelo" } },
                DateDisplay = "1889",
                TypeDisplay = "Marble sculpture",
                Materials = new List<string> { "Marble" },
                Thumbnail = new Image { Url = "https://placehold.co/300x500/94a3b8/1f2937?text=Michelangelo" },
                Subjects = new List<string> { "Human Figure", "Biblical" }
            });

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CHICAGO_ART_INSTITUTE,
                SourceId = 108,
                Type = ArtworkType.Painting,
                Title = "Mona Lisa",
                Artists = new List<Artist> { new Artist { Name = "Leonardo da Vinci" } },
                DateDisplay = "1889",
                TypeDisplay = "Oil on poplar panel",
                Materials = new List<string> { "Oil paint", "Wood" },
                Thumbnail = new Image { Url = "https://placehold.co/400x500/92400e/f9fafb?text=Mona+Lisa" },
                Subjects = new List<string> { "Portrait" }
            });

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CHICAGO_ART_INSTITUTE,
                SourceId = 209,
                Type = ArtworkType.Painting,
                Title = "The Scream",
                Artists = new List<Artist> { new Artist { Name = "Edvard Munch" } },
                DateDisplay = "1889",
                TypeDisplay = "Oil, tempera, and pastel on cardboard",
                Materials = new List<string> { "Oil paint", "Cardboard" },
                Thumbnail = new Image { Url = "https://placehold.co/400x500/fca5a5/450a0a?text=The+Scream" },
                Subjects = new List<string> { "Emotional State", "Anxiety" }
            });

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CLEVELAND_MUSEUM,
                SourceId = 110,
                Type = ArtworkType.Print,
                Title = "The Great Wave off Kanagawa",
                Artists = new List<Artist> { new Artist { Name = "Katsushika Hokusai" } },
                DateDisplay = "1889",
                TypeDisplay = "Woodblock print",
                Materials = new List<string> { "Wood", "Paper" },
                Thumbnail = new Image { Url = "https://placehold.co/500x300/67e8f9/0e7490?text=Hokusai" },
                Subjects = new List<string> { "Landscape", "Nature" }
            });

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CLEVELAND_MUSEUM,
                SourceId = 111,
                Type = ArtworkType.Painting,
                Title = "The Last Supper",
                Artists = new List<Artist> { new Artist { Name = "Leonardo da Vinci" } },
                DateDisplay = "1889",
                TypeDisplay = "Tempera on gesso, pitch, and mastic",
                Materials = new List<string> { "Tempera paint", "Gesso", "Plaster" },
                Thumbnail = new Image { Url = "https://placehold.co/500x300/bfdbfe/1e40af?text=Da+Vinci" },
                Subjects = new List<string> { "Biblical", "Religious" }
            });

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CLEVELAND_MUSEUM,
                SourceId = 212,
                Type = ArtworkType.Painting,
                Title = "Guernica",
                Artists = new List<Artist> { new Artist { Name = "Pablo Picasso" } },
                DateDisplay = "1889",
                TypeDisplay = "Oil on canvas",
                Materials = new List<string> { "Oil paint", "Canvas" },
                Thumbnail = new Image { Url = "https://placehold.co/500x300/e7e5e4/44403c?text=Picasso" },
                Subjects = new List<string> { "War", "Politics" }
            });

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CHICAGO_ART_INSTITUTE,
                SourceId = 113,
                Type = ArtworkType.Painting,
                Title = "American Gothic",
                Artists = new List<Artist> { new Artist { Name = "Grant Wood" } },
                DateDisplay = "1889",
                TypeDisplay = "Oil on beaverboard",
                Materials = new List<string> { "Oil paint", "Wood" },
                Thumbnail = new Image { Url = "https://placehold.co/300x400/e0f2f1/042f2e?text=Wood" },
                Subjects = new List<string> { "Portrait", "Rural Life" }
            });

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CHICAGO_ART_INSTITUTE,
                SourceId = 214,
                Type = ArtworkType.Painting,
                Title = "The Persistence of Memory",
                Artists = new List<Artist> { new Artist { Name = "Salvador Dalí" } },
                DateDisplay = "1889",
                TypeDisplay = "Oil on canvas",
                Materials = new List<string> { "Oil paint", "Canvas" },
                Thumbnail = new Image { Url = "https://placehold.co/400x300/fee2e2/991b1b?text=Dalí" },
                Subjects = new List<string> { "Surrealism", "Time" }
            });
            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CHICAGO_ART_INSTITUTE,
                SourceId = 214,
                Type = ArtworkType.Painting,
                Title = "The Persistence of Memory",
                Artists = new List<Artist> { new Artist { Name = "Salvador Dalí" } },
                DateDisplay = "1889",
                TypeDisplay = "Oil on canvas",
                Materials = new List<string> { "Oil paint", "Canvas" },
                Thumbnail = new Image { Url = "https://placehold.co/400x300/fee2e2/991b1b?text=Dalí" },
                Subjects = new List<string> { "Surrealism", "Time" }
            });
            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CHICAGO_ART_INSTITUTE,
                SourceId = 214,
                Type = ArtworkType.Painting,
                Title = "The Persistence of Memory",
                Artists = new List<Artist> { new Artist { Name = "Salvador Dalí" } },
                DateDisplay = "1889",
                TypeDisplay = "Oil on canvas",
                Materials = new List<string> { "Oil paint", "Canvas" },
                Thumbnail = new Image { Url = "https://placehold.co/400x300/fee2e2/991b1b?text=Dalí" },
                Subjects = new List<string> { "Surrealism", "Time" }
            });
            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CHICAGO_ART_INSTITUTE,
                SourceId = 214,
                Type = ArtworkType.Painting,
                Title = "The Persistence of Memory",
                Artists = new List<Artist> { new Artist { Name = "Salvador Dalí" } },
                DateDisplay = "1889",
                TypeDisplay = "Oil on canvas",
                Materials = new List<string> { "Oil paint", "Canvas" },
                Thumbnail = new Image { Url = "https://placehold.co/400x300/fee2e2/991b1b?text=Dalí" },
                Subjects = new List<string> { "Surrealism", "Time" }
            });
            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CHICAGO_ART_INSTITUTE,
                SourceId = 214,
                Type = ArtworkType.Painting,
                Title = "The Persistence of Memory",
                Artists = new List<Artist> { new Artist { Name = "Salvador Dalí" } },
                DateDisplay = "1889",
                TypeDisplay = "Oil on canvas",
                Materials = new List<string> { "Oil paint", "Canvas" },
                Thumbnail = new Image { Url = "https://placehold.co/400x300/fee2e2/991b1b?text=Dalí" },
                Subjects = new List<string> { "Surrealism", "Time" }
            });



            return artworkPreviews;
        }
    }
}
