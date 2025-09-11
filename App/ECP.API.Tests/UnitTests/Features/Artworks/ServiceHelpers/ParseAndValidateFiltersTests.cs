using System.Net;

namespace ECP.API.Tests.UnitTests.Features.Artworks.ServiceHelpers
{
    [TestFixture]
    internal class ParseAndValidateFiltersTests
    {
        private TestableArtworksService _service;

        [SetUp]
        public void SetUp()
        {
            _service = new();
        }

        [Test]
        public void ParseAndValidateFilters_ValidSingleFilter_ReturnsSuccess()
        {
            // Arrange
            var filterQueries = new List<string> { "artist:Monet" };

            // Act
            var result = _service.ParseAndValidateFilters(filterQueries);
            Console.WriteLine(result.IsSuccess);
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.ContainsKey("artist"), Is.True);
            Assert.That(result.Value["artist"].Contains("monet"), Is.True);
        }

        [Test]
        public void ParseAndValidateFilters_MultipleFiltersSameField_ReturnsSuccess()
        {
            // Arrange
            var filterQueries = new List<string> { "artist:Monet", "artist:Van Gogh" };

            // Act
            var result = _service.ParseAndValidateFilters(filterQueries);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.ContainsKey("artist"), Is.True);
            Assert.That(result.Value["artist"].Count, Is.EqualTo(2));
            Assert.That(result.Value["artist"], Does.Contain("monet"));
            Assert.That(result.Value["artist"], Does.Contain("van gogh"));
        }

        [Test]
        public void ParseAndValidateFilters_MultipleDifferentFields_ReturnsSuccess()
        {
            // Arrange
            var filterQueries = new List<string> { "artist:Monet", "type:painting" };

            // Act
            var result = _service.ParseAndValidateFilters(filterQueries);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.ContainsKey("artist"), Is.True);
            Assert.That(result.Value.ContainsKey("type"), Is.True);
            Assert.That(result.Value["artist"], Does.Contain("monet"));
            Assert.That(result.Value["type"], Does.Contain("painting"));
        }

        [Test]
        public void ParseAndValidateFilters_InvalidFormat_ReturnsFailure()
        {
            // Arrange
            var filterQueries = new List<string> { "artist-Monet" };

            // Act
            var result = _service.ParseAndValidateFilters(filterQueries);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Does.Contain("Invalid filter format"));
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void ParseAndValidateFilters_UnsupportedField_ReturnsFailure()
        {
            // Arrange
            var filterQueries = new List<string> { "unsupported:value" };

            // Act
            var result = _service.ParseAndValidateFilters(filterQueries);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Does.Contain("No valid filters were found. Unsupported field: 'unsupported'. Supported fields are: artist, date, subject, type, material."));
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void ParseAndValidateFilters_NullOrEmptyList_ReturnsFailure()
        {
            // Arrange
            List<string> filterQueries = null;

            // Act
            var result = _service.ParseAndValidateFilters(filterQueries);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Does.Contain("Sort field cannot be empty or null. Please add a valid filter value or remove the parameter."));
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}
