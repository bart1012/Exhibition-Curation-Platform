using System.Net;

namespace ECP.API.Tests.UnitTests.Features.Artworks.ServiceHelpers
{
    [TestFixture]

    internal class ParseAndValidateSortQueryTests
    {
        private TestableArtworksService _service;

        [SetUp]
        public void Setup()
        {
            _service = new TestableArtworksService();
        }

        [Test]
        public void ParseAndValidateSortField_ValidAscendingSort_ReturnsSuccess()
        {
            // Arrange
            var sortQuery = "+title";

            // Act
            var result = _service.ParseAndValidateSortField(sortQuery);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Item1, Is.EqualTo("title"));
            Assert.That(result.Value.Item2, Is.EqualTo('+'));
        }

        [Test]
        public void ParseAndValidateSortField_ValidDescendingSort_ReturnsSuccess()
        {
            // Arrange
            var sortQuery = "-date";

            // Act
            var result = _service.ParseAndValidateSortField(sortQuery);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Item1, Is.EqualTo("date"));
            Assert.That(result.Value.Item2, Is.EqualTo('-'));
        }

        [Test]
        public void ParseAndValidateSortField_InvalidSortField_ReturnsFailure()
        {
            // Arrange
            var sortQuery = "+unsupported";

            // Act
            var result = _service.ParseAndValidateSortField(sortQuery);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Does.Contain("Invalid sort field"));
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void ParseAndValidateSortField_MissingOrderChar_ReturnsFailure()
        {
            // Arrange
            var sortQuery = "title";

            // Act
            var result = _service.ParseAndValidateSortField(sortQuery);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Does.Contain("Please specify the sort order"));
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void ParseAndValidateSortField_NullOrEmptyQuery_ReturnsFailure()
        {
            // Arrange
            string sortQuery = null;

            // Act
            var result = _service.ParseAndValidateSortField(sortQuery);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Does.Contain("Sort field cannot be empty or null"));
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}
