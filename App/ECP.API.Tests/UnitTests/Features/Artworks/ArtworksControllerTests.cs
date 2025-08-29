using ECP.API.Features.Artworks;
using ECP.API.Features.Artworks.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ECP.API.Tests.UnitTests.Features.Artworks
{
    public class ArtworksControllerTests
    {
        private Mock<IArtworksService> _mockArtworksService;
        private ArtworksController _artworksController;

        [SetUp]
        public void Setup()
        {
            _mockArtworksService = new Mock<IArtworksService>();
            _artworksController = new ArtworksController(_mockArtworksService.Object);
        }

        [Test]
        public async Task GetArtworkPreviewAsync_WhenServiceReturnsSuccessWithData_ReturnsOkResultWithArtworks()
        {
            // Arrange
            var artworkList = new List<ArtworkPreview>()
            {
                new ArtworkPreview()
                {
                    Id = "artwork_0",
                    Source = ArtworkSource.CLEVELAND_MUSEUM,
                    SourceId = 1012,
                    Title = "Artwork_0",
                    Artists = new List<Artist>()
                    {
                        new Artist(){Name="Sample_Artist_0"}
                    },
                    WebImage = new Image(){Url="url_0",Width=1080, Height=920}
                },
                new ArtworkPreview()
                {
                    Id = "artwork_1",
                    Source = ArtworkSource.CHICAGO_ART_INSTITUTE,
                    SourceId = 2032,
                    Title = "Artwork_1",
                    Artists = new List<Artist>()
                    {
                        new Artist(){Name="Sample_Artist_1"}
                    },
                    WebImage = new Image(){Url="url_1",Width=1080, Height=920}
                }
            };
            var successResult = Shared.Result<List<ArtworkPreview>>.Success(artworkList);
            var completedTask = Task.FromResult(successResult);
            _mockArtworksService.Setup(service => service.GetArtworkPreviewAsync(2)).Returns(completedTask);

            // Act
            var result = await _artworksController.GetArtworkPreviewAsync(2);

            // Assert
            _mockArtworksService.Verify(service => service.GetArtworkPreviewAsync(2), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value.Should().NotBeNull();
            var resultValue = okResult.Value as List<ArtworkPreview>;
            resultValue.Should().BeEquivalentTo(new List<ArtworkPreview>()
            {
                 new ArtworkPreview()
                {
                    Id = "artwork_0",
                    Source = ArtworkSource.CLEVELAND_MUSEUM,
                    SourceId = 1012,
                    Title = "Artwork_0",
                    Artists = new List<Artist>()
                    {
                        new Artist(){Name="Sample_Artist_0"}
                    },
                    WebImage = new Image(){Url="url_0",Width=1080, Height=920}
                },
                new ArtworkPreview()
                {
                    Id = "artwork_1",
                    Source = ArtworkSource.CHICAGO_ART_INSTITUTE,
                    SourceId = 2032,
                    Title = "Artwork_1",
                    Artists = new List<Artist>()
                    {
                        new Artist(){Name="Sample_Artist_1"}
                    },
                    WebImage = new Image(){Url="url_1",Width=1080, Height=920}
                }

            });
        }

        [Test]
        public async Task GetArtworkPreviewAsync_WhenServiceReturnsSuccessWithEmptyList_ReturnsNoContentResult()
        {

            // Arrange
            List<ArtworkPreview> artworkList = new();
            var successResult = Shared.Result<List<ArtworkPreview>>.Success(artworkList);
            var completedTask = Task.FromResult(successResult);
            _mockArtworksService.Setup(service => service.GetArtworkPreviewAsync(2)).Returns(completedTask);

            // Act
            var result = await _artworksController.GetArtworkPreviewAsync(2);

            // Assert
            _mockArtworksService.Verify(service => service.GetArtworkPreviewAsync(2), Times.Once);
            result.Should().BeOfType<NoContentResult>();
            var okResult = result as NoContentResult;
            okResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Test]
        public async Task GetArtworkPreviewAsync_WhenServiceReturnsSuccessWithNullValue_ReturnsNoContentResult()
        {
            // Arrange
            List<ArtworkPreview> artworkList = null;
            var successResult = Shared.Result<List<ArtworkPreview>>.Success(null);
            var completedTask = Task.FromResult(successResult);
            _mockArtworksService.Setup(service => service.GetArtworkPreviewAsync(2)).Returns(completedTask);

            // Act
            var result = await _artworksController.GetArtworkPreviewAsync(2);

            // Assert
            _mockArtworksService.Verify(service => service.GetArtworkPreviewAsync(2), Times.Once);
            result.Should().BeOfType<NoContentResult>();
            var okResult = result as NoContentResult;
            okResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Test]
        public async Task GetArtworkPreviewAsync_WhenServiceReturnsFailure_ReturnsInternalServerError()
        {
            // Arrange
            var failureResult = Shared.Result<List<ArtworkPreview>>.Failure("Error connecting to external APIs", System.Net.HttpStatusCode.Unauthorized);
            var completedTask = Task.FromResult(failureResult);
            _mockArtworksService.Setup(service => service.GetArtworkPreviewAsync(2)).Returns(completedTask);

            // Act
            var result = await _artworksController.GetArtworkPreviewAsync(2);

            // Assert
            _mockArtworksService.Verify(service => service.GetArtworkPreviewAsync(2), Times.Once);
            var serverErrorResult = result as ObjectResult;
            serverErrorResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            serverErrorResult.Value.Should().BeEquivalentTo(new { error = "Error connecting to external APIs" });
        }
    }
}
