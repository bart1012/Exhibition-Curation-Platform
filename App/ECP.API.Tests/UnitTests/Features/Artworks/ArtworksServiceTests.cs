using ECP.API.Features.Artworks;
using ECP.API.Features.Artworks.Models;
using FluentAssertions;
using Moq;

namespace ECP.API.Tests.UnitTests.Features.Artworks
{
    internal class ArtworksServiceTests
    {
        private Mock<IArtworksRepository> _mockArtworksRepository;
        private ArtworksService _artworksService;

        [SetUp]
        public void Setup()
        {
            _mockArtworksRepository = new Mock<IArtworksRepository>();
            _artworksService = new ArtworksService(_mockArtworksRepository.Object);
        }

        [Test]
        public async Task GetArtworkPreviewAsync_WhenRepositoryReturnsSuccessWithData_ReturnsSuccessWithData()
        {
            // Arrange
            var expectedCount = 5;
            var artworkList = new List<ArtworkPreview>
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
            var repositoryResult = Shared.Result<List<ArtworkPreview>>.Success(artworkList);

            _mockArtworksRepository.Setup(r => r.GetArtworkPreviewAsync(expectedCount))
                .ReturnsAsync(repositoryResult);

            // Act
            var serviceResult = await _artworksService.GetArtworkPreviewAsync(expectedCount);

            // Assert
            serviceResult.IsSuccess.Should().BeTrue();
            serviceResult.Value.Should().BeEquivalentTo(artworkList);
            _mockArtworksRepository.Verify(r => r.GetArtworkPreviewAsync(expectedCount), Times.Once);
        }

        [Test]
        public async Task GetArtworkPreviewAsync_WhenRepositoryReturnsEmptyList_ReturnsEmptyList()
        {
            // Arrange
            var expectedCount = 5;
            var emptyList = new List<ArtworkPreview>();
            var repositoryResult = Shared.Result<List<ArtworkPreview>>.Success(emptyList);

            _mockArtworksRepository.Setup(r => r.GetArtworkPreviewAsync(expectedCount))
                .ReturnsAsync(repositoryResult);

            // Act
            var serviceResult = await _artworksService.GetArtworkPreviewAsync(expectedCount);

            // Assert
            serviceResult.IsSuccess.Should().BeTrue();
            serviceResult.Value.Should().BeEmpty();
            _mockArtworksRepository.Verify(r => r.GetArtworkPreviewAsync(expectedCount), Times.Once);
        }

        [Test]
        public async Task GetArtworkPreviewAsync_WhenRepositoryReturnsFailure_ReturnsFailure()
        {
            // Arrange
            var expectedCount = 5;
            var expectedError = "Repository connection failed.";
            var repositoryResult = Shared.Result<List<ArtworkPreview>>.Failure(expectedError, System.Net.HttpStatusCode.InternalServerError);

            _mockArtworksRepository.Setup(r => r.GetArtworkPreviewAsync(expectedCount))
                .ReturnsAsync(repositoryResult);

            // Act
            var serviceResult = await _artworksService.GetArtworkPreviewAsync(expectedCount);

            // Assert
            serviceResult.IsSuccess.Should().BeFalse();
            serviceResult.ErrorMessage.Should().Be(expectedError);
            serviceResult.Value.Should().BeNull();
            _mockArtworksRepository.Verify(r => r.GetArtworkPreviewAsync(expectedCount), Times.Once);
        }

        [Test]
        public async Task GetArtworkPreviewByQueryAsync_WhenRepositoryReturnsSuccess_ReturnsSuccessWithData()
        {
            // Arrange
            var expectedQuery = "monet";
            var expectedCount = 10;
            var artworkList = new List<ArtworkPreview>
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
            var repositoryResult = Shared.Result<List<ArtworkPreview>>.Success(artworkList);

            _mockArtworksRepository.Setup(r => r.GetArtworkPreviewByQueryAsync(expectedQuery, expectedCount))
                .ReturnsAsync(repositoryResult);

            // Act
            var serviceResult = await _artworksService.GetArtworkPreviewByQueryAsync(expectedQuery, expectedCount);

            // Assert
            serviceResult.IsSuccess.Should().BeTrue();
            serviceResult.Value.Should().BeEquivalentTo(artworkList);
            _mockArtworksRepository.Verify(r => r.GetArtworkPreviewByQueryAsync(expectedQuery, expectedCount), Times.Once);
        }

        [Test]
        public async Task GetArtworkPreviewByQueryAsync_WhenRepositoryReturnsEmptyList_ReturnsEmptyList()
        {
            // Arrange
            var expectedQuery = "unknown artist";
            var expectedCount = 10;
            var emptyList = new List<ArtworkPreview>();
            var repositoryResult = Shared.Result<List<ArtworkPreview>>.Success(emptyList);

            _mockArtworksRepository.Setup(r => r.GetArtworkPreviewByQueryAsync(expectedQuery, expectedCount))
                .ReturnsAsync(repositoryResult);

            // Act
            var serviceResult = await _artworksService.GetArtworkPreviewByQueryAsync(expectedQuery, expectedCount);

            // Assert
            serviceResult.IsSuccess.Should().BeTrue();
            serviceResult.Value.Should().BeEmpty();
            _mockArtworksRepository.Verify(r => r.GetArtworkPreviewByQueryAsync(expectedQuery, expectedCount), Times.Once);
        }

        [Test]
        public async Task GetArtworkPreviewByQueryAsync_WhenRepositoryReturnsFailure_ReturnsFailure()
        {
            // Arrange
            var expectedQuery = "monet";
            var expectedCount = 10;
            var expectedError = "Search service is unavailable.";
            var repositoryResult = Shared.Result<List<ArtworkPreview>>.Failure(expectedError, System.Net.HttpStatusCode.ServiceUnavailable);

            _mockArtworksRepository.Setup(r => r.GetArtworkPreviewByQueryAsync(expectedQuery, expectedCount))
                .ReturnsAsync(repositoryResult);

            // Act
            var serviceResult = await _artworksService.GetArtworkPreviewByQueryAsync(expectedQuery, expectedCount);

            // Assert
            serviceResult.IsSuccess.Should().BeFalse();
            serviceResult.ErrorMessage.Should().Be(expectedError);
            serviceResult.Value.Should().BeNull();
            _mockArtworksRepository.Verify(r => r.GetArtworkPreviewByQueryAsync(expectedQuery, expectedCount), Times.Once);
        }
    }
}
