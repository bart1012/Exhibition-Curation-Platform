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
                WebImage = new Image { Url = "https://upload.wikimedia.org/wikipedia/commons/e/ea/Van_Gogh_-_Starry_Night_-_Google_Art_Project.jpg" },
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
                WebImage = new Image { Url = "https://www.rodinmuseum.org/assets/img/collection/103355_0.jpg" },
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
                WebImage = new Image { Url = "https://upload.wikimedia.org/wikipedia/commons/5/54/Dorothea_Lange%2C_Migrant_Mother_02.jpg" },
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
                WebImage = new Image { Url = "https://upload.wikimedia.org/wikipedia/commons/0/0f/1665_Girl_with_a_Pearl_Earring.jpg" },
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
                WebImage = new Image { Url = "https://upload.wikimedia.org/wikipedia/commons/2/22/Leonardo_da_Vinci_-_Vitruvian_Man.jpg" },
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
                WebImage = new Image { Url = "https://upload.wikimedia.org/wikipedia/commons/5/59/Monet_-_Impression%2C_Sunrise.jpg" },
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
                WebImage = new Image { Url = "https://upload.wikimedia.org/wikipedia/commons/4/47/Michelangelos_David_in_der_Galleria_dell%27Accademia.jpg" },
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
                WebImage = new Image { Url = "https://upload.wikimedia.org/wikipedia/commons/e/ec/Mona_Lisa%2C_by_Leonardo_da_Vinci%2C_from_C2RMF_retouched.jpg" },
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
                WebImage = new Image { Url = "https://upload.wikimedia.org/wikipedia/commons/c/c5/Edvard_Munch%2C_The_Scream%2C_1893.jpg" },
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
                WebImage = new Image { Url = "https://upload.wikimedia.org/wikipedia/commons/a/a2/The_Great_Wave_off_Kanagawa_by_Hokusai.jpg" },
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
                WebImage = new Image { Url = "https://upload.wikimedia.org/wikipedia/commons/4/4b/Leonardo_da_Vinci_-_The_Last_Supper.jpg" },
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
                WebImage = new Image { Url = "https://upload.wikimedia.org/wikipedia/commons/1/1a/Picasso%2C_Guernica.jpg" },
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
                WebImage = new Image { Url = "https://upload.wikimedia.org/wikipedia/commons/c/c2/American_Gothic.jpg" },
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
                WebImage = new Image { Url = "https://upload.wikimedia.org/wikipedia/commons/d/d3/The_Persistence_of_Memory.jpg" },
                Subjects = new List<string> { "Surrealism", "Time" }
            });

            artworkPreviews.Add(new ArtworkPreview
            {
                Id = Guid.NewGuid().ToString(),
                Source = ArtworkSource.CHICAGO_ART_INSTITUTE,
                SourceId = 115,
                ArtworkType = ArtworkType.Painting,
                Title = "The Birth of Venus",
                Artists = new List<Artist> { new Artist { Name = "Sandro Botticelli" } },
                CreationYear = 1486,
                ArtworkTypeDisplay = "Tempera on canvas",
                Materials = new List<string> { "Tempera paint", "Canvas" },
                WebImage = new Image { Url = "https://upload.wikimedia.org/wikipedia/commons/0/0b/Sandro_Botticelli_-_La_nascita_di_Venere_-_Google_Art_Project_-_edited.jpg" },
                Subjects = new List<string> { "Mythology", "Classical" }
            });

            return artworkPreviews;
        }
    }
}
