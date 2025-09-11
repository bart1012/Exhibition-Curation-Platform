using ECP.API.Features.Artworks;
using ECP.Shared;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace ECP.API.Tests.UnitTests.Features.Artworks
{
    internal class ArtworksServiceTests
    {
        private Mock<IArtworksRepository> _mockArtworksRepository;
        private Mock<IMemoryCache> _mockMemoryCache;
        private ArtworksService _artworksService;
        private Mock<ICacheEntry> _mockCacheEntry;

        [SetUp]
        public void Setup()
        {
            _mockArtworksRepository = new Mock<IArtworksRepository>();
            _mockMemoryCache = new Mock<IMemoryCache>();
            _mockCacheEntry = new Mock<ICacheEntry>();
            _artworksService = new ArtworksService(_mockArtworksRepository.Object, _mockMemoryCache.Object);
        }

        [Test]
        public async Task GetArtworkPreviewAsync_WhenRepositoryReturnsSuccessWithData_ReturnsPaginatedResponseWithData()
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

            _mockArtworksRepository.Setup(r => r.GetArtworkPreviewsAsync(expectedCount))
                .ReturnsAsync(repositoryResult);

            // Act
            var serviceResult = await _artworksService.GetArtworkPreviewsAsync(expectedCount);

            // Assert
            serviceResult.IsSuccess.Should().BeTrue();
            serviceResult.Value.Should().BeEquivalentTo(new PaginatedResponse<ArtworkPreview>()
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
            _mockArtworksRepository.Verify(r => r.GetArtworkPreviewsAsync(expectedCount), Times.Once);
        }

        [Test]
        public async Task GetArtworkPreviewAsync_WhenRepositoryReturnsEmptyList_ReturnsEmptyPaginatedResponse()
        {
            // Arrange
            var expectedCount = 5;
            var emptyList = new List<ArtworkPreview>();
            var repositoryResult = Shared.Result<List<ArtworkPreview>>.Success(emptyList);

            _mockArtworksRepository.Setup(r => r.GetArtworkPreviewsAsync(expectedCount))
                .ReturnsAsync(repositoryResult);

            // Act
            var serviceResult = await _artworksService.GetArtworkPreviewsAsync(expectedCount);

            // Assert
            serviceResult.IsSuccess.Should().BeTrue();
            serviceResult.Value.Should().BeEquivalentTo(new PaginatedResponse<ArtworkPreview>()
            {
                Data = new List<ArtworkPreview>(),
                Info = new PaginationInfo()
                {
                    ItemsPerPage = 25,
                    TotalItems = 0,
                    CurrentPage = 1,
                    TotalPages = 0
                }
            });
            _mockArtworksRepository.Verify(r => r.GetArtworkPreviewsAsync(expectedCount), Times.Once);
        }

        [Test]
        public async Task GetArtworkPreviewAsync_WhenRepositoryReturnsFailure_ReturnsFailure()
        {
            // Arrange
            var expectedCount = 5;
            var expectedError = "Repository connection failed.";
            var repositoryResult = Shared.Result<List<ArtworkPreview>>.Failure(expectedError, System.Net.HttpStatusCode.InternalServerError);

            _mockArtworksRepository.Setup(r => r.GetArtworkPreviewsAsync(expectedCount))
                .ReturnsAsync(repositoryResult);

            // Act
            var serviceResult = await _artworksService.GetArtworkPreviewsAsync(expectedCount);

            // Assert
            serviceResult.IsSuccess.Should().BeFalse();
            serviceResult.Message.Should().Be(expectedError);
            serviceResult.Value.Should().BeNull();
            _mockArtworksRepository.Verify(r => r.GetArtworkPreviewsAsync(expectedCount), Times.Once);
        }

        [Test]
        public async Task SearchAllArtworkPreviewsAsync_WhenRepositoryReturnsSuccess_ReturnsPaginatedResponseWithData()
        {
            // Arrange
            var expectedQuery = "monet";
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

            _mockArtworksRepository.Setup(r => r.SearchAllArtworkPreviewsAsync(expectedQuery))
                .ReturnsAsync(repositoryResult);

            object cacheValue = null;

            _mockMemoryCache.Setup(c => c.TryGetValue(It.IsAny<object>(), out cacheValue))
            .Returns(false);

            _mockMemoryCache.Setup(m => m.CreateEntry(It.IsAny<object>()))
             .Returns(_mockCacheEntry.Object);

            // Act
            var serviceResult = await _artworksService.SearchAllArtworkPreviewsAsync(expectedQuery, null, null, 25, 1);

            // Assert
            serviceResult.IsSuccess.Should().BeTrue();
            var expectation = new PaginatedResponse<ArtworkPreview>()
            {
                Data = artworkList,
                Info = new PaginationInfo()
                {
                    ItemsPerPage = 25,
                    TotalItems = 2,
                    CurrentPage = 1,
                    TotalPages = 1
                }
            };
            serviceResult.Value.Should().BeEquivalentTo(expectation);
            _mockArtworksRepository.Verify(r => r.SearchAllArtworkPreviewsAsync(expectedQuery), Times.Once);
        }

        [Test]
        public async Task SearchAllArtworkPreviewsAsync_WhenRepositoryReturnsEmptyList_ReturnsEmptyPaginatedResponse()
        {
            // Arrange
            var expectedQuery = "unknown artist";
            var expectedCount = 10;
            var expectedOffset = 1;
            var emptyList = new List<ArtworkPreview>();
            var repositoryResult = Shared.Result<List<ArtworkPreview>>.Success(emptyList);

            _mockArtworksRepository.Setup(r => r.SearchAllArtworkPreviewsAsync(expectedQuery))
                .ReturnsAsync(repositoryResult);

            // Act
            var serviceResult = await _artworksService.SearchAllArtworkPreviewsAsync(expectedQuery);

            // Assert
            serviceResult.IsSuccess.Should().BeTrue();
            serviceResult.Value.Should().BeEquivalentTo(new PaginatedResponse<ArtworkPreview>()
            {
                Data = new List<ArtworkPreview>(),
                Info = new PaginationInfo()
                {
                    ItemsPerPage = 25,
                    TotalItems = 0,
                    CurrentPage = 1,
                    TotalPages = 0
                }
            });
            _mockArtworksRepository.Verify(r => r.SearchAllArtworkPreviewsAsync(expectedQuery), Times.Once);
        }

        [Test]
        public async Task SearchAllArtworkPreviewsAsync_WhenRepositoryReturnsFailure_ReturnsFailure()
        {
            // Arrange
            var expectedQuery = "monet";
            var expectedCount = 10;
            var expectedOffset = 1;
            var expectedError = "Search service is unavailable.";
            var repositoryResult = Shared.Result<List<ArtworkPreview>>.Failure(expectedError, System.Net.HttpStatusCode.ServiceUnavailable);

            _mockArtworksRepository.Setup(r => r.SearchAllArtworkPreviewsAsync(expectedQuery))
                .ReturnsAsync(repositoryResult);

            // Act
            var serviceResult = await _artworksService.SearchAllArtworkPreviewsAsync(expectedQuery);

            // s
            serviceResult.IsSuccess.Should().BeFalse();
            serviceResult.Message.Should().Be(expectedError);
            serviceResult.Value.Should().BeNull();
            _mockArtworksRepository.Verify(r => r.SearchAllArtworkPreviewsAsync(expectedQuery), Times.Once);
        }

    }
}
