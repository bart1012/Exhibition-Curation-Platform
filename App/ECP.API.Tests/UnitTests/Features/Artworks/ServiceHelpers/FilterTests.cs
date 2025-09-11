using ECP.API.Tests.UnitTests.Features.Artworks.ServiceHelpers;
using ECP.Shared;
using System.Net;

namespace ECP.API.Tests.UnitTests.Features.Artworks.Helpers
{
    internal class FilterTests
    {
        private TestableArtworksService _service;
        private List<ArtworkPreview> _testData;

        [SetUp]
        public void Setup()
        {
            _service = new TestableArtworksService();
            _testData = new List<ArtworkPreview>
        {
            new ArtworkPreview { Title = "Starry Night", Artists = new List<Artist> { new Artist() { Name = "Van Gogh" } }, ArtworkType = ArtworkType.Painting, Materials = new List<string> { "oil", "canvas" }, Subjects = new List<string> { "landscape" } },
            new ArtworkPreview { Title = "The Persistence of Memory", Artists = new List<Artist> { new Artist() { Name = "Dali" } }, ArtworkType = ArtworkType.Painting, Materials = new List<string> { "oil", "canvas" }, Subjects = new List<string> { "surrealism" } },
            new ArtworkPreview { Title = "The Thinker", Artists = new List<Artist> { new Artist() { Name = "Rodin" } }, ArtworkType = ArtworkType.Sculpture, Materials = new List<string> { "bronze" }, Subjects = new List<string> { "figure" } },
            new ArtworkPreview { Title = "Water Lilies", Artists = new List<Artist> { new Artist() { Name = "Monet" } }, ArtworkType = ArtworkType.Painting, Materials = new List<string> { "oil", "canvas" }, Subjects = new List<string> { "landscape" } },
            new ArtworkPreview { Title = "Vouquet of Sunflowers", Artists = new List<Artist> { new Artist() { Name = "Monet" } }, ArtworkType = ArtworkType.Painting, Materials = new List<string> { "oil", "canvas" }, Subjects = new List<string> { "still-life" } },
            };
        }

        [Test]
        public void Filter_ByArtist_ReturnsCorrectResults()
        {
            // Arrange
            var filterQueries = new List<string> { "artist:van gogh" };

            // Act
            var result = _service.Filter(_testData, filterQueries);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Count, Is.EqualTo(1));
            Assert.That(result.Value[0].Title, Is.EqualTo("Starry Night"));
        }

        [Test]
        public void Filter_ByType_ReturnsCorrectResults()
        {
            // Arrange
            var filterQueries = new List<string> { "type:Painting" };

            // Act
            var result = _service.Filter(_testData, filterQueries);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Count, Is.EqualTo(4));
            Assert.That(result.Value.Select(a => a.Title).ToList(), Does.Contain("Starry Night"));
            Assert.That(result.Value.Select(a => a.Title).ToList(), Does.Contain("The Persistence of Memory"));
            Assert.That(result.Value.Select(a => a.Title).ToList(), Does.Contain("Water Lilies"));
        }

        [Test]
        public void Filter_ByMaterial_ReturnsCorrectResults()
        {
            // Arrange
            var filterQueries = new List<string> { "material:bronze" };

            // Act
            var result = _service.Filter(_testData, filterQueries);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Count, Is.EqualTo(1));
            Assert.That(result.Value[0].Title, Is.EqualTo("The Thinker"));
        }

        [Test]
        public void Filter_ByMultipleCriteria_ReturnsIntersection()
        {
            // Arrange
            var filterQueries = new List<string> { "artist:Monet", "subject:landscape" };

            // Act
            var result = _service.Filter(_testData, filterQueries);
            Console.WriteLine(result.Message);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Count, Is.EqualTo(1));
            Assert.That(result.Value[0].Title, Is.EqualTo("Water Lilies"));
        }

        [Test]
        public void Filter_NoMatchingResults_ReturnsEmptyList()
        {
            // Arrange
            var filterQueries = new List<string> { "artist:Picasso" };

            // Act
            var result = _service.Filter(_testData, filterQueries);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Count, Is.EqualTo(0));
        }

        [Test]
        public void Filter_WithInvalidFilterQuery_ReturnsFailure()
        {
            // Arrange
            var supportedFields = new HashSet<string> { "artist", "date", "subject", "type", "material" };
            var filterQueries = new List<string> { "invalid:value" };

            // Act
            var result = _service.Filter(_testData, filterQueries);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Does.Contain($"No valid filters were found. Unsupported field: 'invalid'. Supported fields are: {string.Join(", ", supportedFields)}."));
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}
