using ECP.API.Tests.UnitTests.Features.Artworks.ServiceHelpers;
using ECP.Shared;
using System.Net;

namespace ECP.API.Tests.UnitTests.Features.Artworks.Helpers
{
    [TestFixture]
    public class SortTests
    {
        private TestableArtworksService _service;
        private List<ArtworkPreview> _testData;


        [SetUp]
        public void SetUp()
        {
            _service = new TestableArtworksService();
            _testData = new()
            {
                new ArtworkPreview { Title = "C Title", SortableYear = 2000 },
                new ArtworkPreview { Title = "A Title", SortableYear = 1800 },
                new ArtworkPreview { Title = "B Title", SortableYear = 1900 }
            };
        }

        [Test]
        public void Sort_ByTitleAscending_ReturnsCorrectlySortedList()
        {
            // Arrange
            var sortQuery = "+title";

            // Act
            Result<List<ArtworkPreview>> result = _service.Sort(_testData, sortQuery);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Count, Is.EqualTo(3));
            Assert.That(result.Value.Select(a => a.Title).ToList(), Is.EqualTo(new List<string> { "A Title", "B Title", "C Title" }));
        }

        [Test]
        public void Sort_ByTitleDescending_ReturnsCorrectlySortedList()
        {
            // Arrange
            var sortQuery = "-title";

            // Act
            var result = _service.Sort(_testData, sortQuery);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Count, Is.EqualTo(3));
            Assert.That(result.Value.Select(a => a.Title).ToList(), Is.EqualTo(new List<string> { "C Title", "B Title", "A Title" }));
        }

        [Test]
        public void Sort_ByDateAscending_ReturnsCorrectlySortedList()
        {
            // Arrange
            var sortQuery = "+date";

            // Act
            var result = _service.Sort(_testData, sortQuery);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Count, Is.EqualTo(3));
            Assert.That(result.Value.Select(a => a.SortableYear).ToList(), Is.EqualTo(new List<int> { 1800, 1900, 2000 }));
        }

        [Test]
        public void Sort_ByDateDescending_ReturnsCorrectlySortedList()
        {
            // Arrange
            var sortQuery = "-date";

            // Act
            var result = _service.Sort(_testData, sortQuery);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Count, Is.EqualTo(3));
            Assert.That(result.Value.Select(a => a.SortableYear).ToList(), Is.EqualTo(new List<int> { 2000, 1900, 1800 }));
        }

        [Test]
        public void Sort_WithInvalidSortQuery_ReturnsFailure()
        {
            // Arrange
            var sortQuery = "-invalidfield";

            // Act
            var result = _service.Sort(_testData, sortQuery);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Does.Contain("Invalid sort field"));
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}
