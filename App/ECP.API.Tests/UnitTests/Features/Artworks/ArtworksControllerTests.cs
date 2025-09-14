using ECP.API.Features.Artworks;
using ECP.Shared;
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
        public async Task GetArtworkPreviewAsync_WhenServiceReturnsSuccessWithData_ReturnsOkResultWithPaginatedDataModel()
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
                    Thumbnail = new Image(){Url="url_0",Width=1080, Height=920}
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
                    Thumbnail = new Image(){Url="url_1",Width=1080, Height=920}
                }
            };
            var successResult = Shared.Result<PaginatedResponse<ArtworkPreview>>.Success(new PaginatedResponse<ArtworkPreview>()
            {
                Data = artworkList,
                Info = new PaginationInfo()
                {
                    ItemsPerPage = 25,
                    TotalItems = 2,
                    CurrentPage = 1,
                    TotalPages = 1
                }
            });
            var completedTask = Task.FromResult(successResult);
            _mockArtworksService.Setup(service => service.GetArtworkPreviewsAsync(2, 25, 1)).Returns(completedTask);

            // Act
            var result = await _artworksController.GetArtworkPreviewAsync(2, 25, 1);

            // Assert
            _mockArtworksService.Verify(service => service.GetArtworkPreviewsAsync(2, 25, 1), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value.Should().NotBeNull();
            var resultValue = okResult.Value as PaginatedResponse<ArtworkPreview>;
            resultValue.Should().BeEquivalentTo(new PaginatedResponse<ArtworkPreview>()
            {
                Data = artworkList,
                Info = new PaginationInfo()
                {
                    ItemsPerPage = 25,
                    TotalItems = 2,
                    CurrentPage = 1,
                    TotalPages = 1
                }
            });
        }

        [Test]
        public async Task GetArtworkPreviewAsync_WhenServiceReturnsSuccessWithEmptyList_ReturnsNoContentResult()
        {

            // Arrange
            var paginatedResponse = new PaginatedResponse<ArtworkPreview>()
            {
                Data = new List<ArtworkPreview>(),
                Info = new PaginationInfo()
                {
                    ItemsPerPage = 25,
                    TotalItems = 0,
                    CurrentPage = 1,
                    TotalPages = 0
                }
            };
            var successResult = Shared.Result<PaginatedResponse<ArtworkPreview>>.Success(paginatedResponse);
            var completedTask = Task.FromResult(successResult);
            _mockArtworksService.Setup(service => service.GetArtworkPreviewsAsync(2, 25, 1)).Returns(completedTask);

            // Act
            var result = await _artworksController.GetArtworkPreviewAsync(2, 25, 1);

            // Assert
            _mockArtworksService.Verify(service => service.GetArtworkPreviewsAsync(2, 25, 1), Times.Once);
            result.Should().BeOfType<NoContentResult>();
            var okResult = result as NoContentResult;
            okResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Test]
        public async Task GetArtworkPreviewAsync_WhenServiceReturnsSuccessWithNullValue_ReturnsNoContentResult()
        {
            // Arrange
            var paginatedResponse = new PaginatedResponse<ArtworkPreview>()
            {
                Data = null,
                Info = new PaginationInfo()
                {
                    ItemsPerPage = 25,
                    TotalItems = 0,
                    CurrentPage = 1,
                    TotalPages = 0
                }
            };
            var successResult = Shared.Result<PaginatedResponse<ArtworkPreview>>.Success(paginatedResponse);
            var completedTask = Task.FromResult(successResult);
            _mockArtworksService.Setup(service => service.GetArtworkPreviewsAsync(2, 25, 1)).Returns(completedTask);

            // Act
            var result = await _artworksController.GetArtworkPreviewAsync(2, 25, 1);

            // Assert
            _mockArtworksService.Verify(service => service.GetArtworkPreviewsAsync(2, 25, 1), Times.Once);
            result.Should().BeOfType<NoContentResult>();
            var okResult = result as NoContentResult;
            okResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Test]
        public async Task GetArtworkPreviewAsync_WhenServiceReturnsFailure_ReturnsInternalServerError()
        {
            // Arrange
            var failureResult = Shared.Result<PaginatedResponse<ArtworkPreview>>.Failure("Error connecting to external APIs", System.Net.HttpStatusCode.Unauthorized);
            var completedTask = Task.FromResult(failureResult);
            _mockArtworksService.Setup(service => service.GetArtworkPreviewsAsync(2, 25, 1)).Returns(completedTask);

            // Act
            var result = await _artworksController.GetArtworkPreviewAsync(2, 25, 1);

            // Assert
            _mockArtworksService.Verify(service => service.GetArtworkPreviewsAsync(2, 25, 1), Times.Once);
            var serverErrorResult = result as ObjectResult;
            serverErrorResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            serverErrorResult.Value.Should().BeEquivalentTo(new { error = "Error connecting to external APIs" });
        }
    }
}
