using ECP.API.Features.Artworks;
using ECP.API.Features.Artworks.Clients.ChicagoArtInstitute;
using ECP.API.Features.Artworks.Clients.ChicagoArtInstitute.Models;
using ECP.API.Features.Artworks.Clients.ClevelandMuseum;
using ECP.API.Features.Artworks.Clients.ClevelandMuseum.Models;
using ECP.Shared;
using FluentAssertions;
using Moq;

namespace ECP.API.Tests.UnitTests.Features.Artworks
{
    public class ArtworksRepositoryTests
    {
        private Mock<IClevelandMuseumClient> _mockClevelandClient;
        private Mock<IChicagoArtInstituteClient> _mockChicagoClient;
        private Mock<IArtworkMapper> _mockMapper;
        private ArtworksRepository _artworksRepository;

        [SetUp]
        public void Setup()
        {
            _mockClevelandClient = new Mock<IClevelandMuseumClient>();
            _mockChicagoClient = new Mock<IChicagoArtInstituteClient>();
            _mockMapper = new Mock<IArtworkMapper>();
            _artworksRepository = new ArtworksRepository(_mockClevelandClient.Object, _mockChicagoClient.Object, _mockMapper.Object);
        }

        private List<ClevelandArtworkPreview> CreateClevelandArtPreviews(int count)
        {
            return Enumerable.Range(0, count)
                .Select(i => new ClevelandArtworkPreview
                {
                    Id = 1000 + i,
                    Title = $"ClevelandImage_{i}"
                }).ToList();
        }
        private List<ChicagoArtworkPreview> CreateChicagoArtPreviews(int count)
        {
            return Enumerable.Range(0, count)
                .Select(i => new ChicagoArtworkPreview
                {
                    Id = 2000 + i,
                    Title = $"ChicagoImage_{i}"
                }).ToList();
        }

        private void SetupMapper(List<ClevelandArtworkPreview> clevelandArtwork, List<ChicagoArtworkPreview> chicagoArtwork)
        {
            foreach (var artwork in clevelandArtwork)
            {
                _mockMapper.Setup(m => m.FromClevelandPreview(artwork))
                    .Returns(new ArtworkPreview { Id = string.Concat("cleveland_", artwork.Id.ToString()), Source = ArtworkSource.CLEVELAND_MUSEUM, Title = artwork.Title, Thumbnail = new() });
            }
            foreach (var artwork in chicagoArtwork)
            {
                _mockMapper.Setup(m => m.FromChicagoPreview(artwork))
                    .Returns(new ArtworkPreview { Id = string.Concat("chicago", artwork.Id.ToString()), Source = ArtworkSource.CHICAGO_ART_INSTITUTE, Title = artwork.Title, Thumbnail = new() });
            }
        }

        [Test]
        public async Task GetArtworkPreviewAsync_WhenBothClientsReturnData_ReturnsCombinedList()
        {
            // Arrange
            var clevelandArtworks = CreateClevelandArtPreviews(2);
            var chicagoArtworks = CreateChicagoArtPreviews(2);

            _mockClevelandClient.Setup(c => c.GetArtworkPreviews(2)).ReturnsAsync(clevelandArtworks);
            _mockChicagoClient.Setup(c => c.GetArtworkPreviews(2)).ReturnsAsync(chicagoArtworks);
            SetupMapper(clevelandArtworks, chicagoArtworks);

            // Act
            var result = await _artworksRepository.GetArtworkPreviewsAsync(2);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().HaveCount(4);
            _mockClevelandClient.Verify(c => c.GetArtworkPreviews(2), Times.Once);
            _mockChicagoClient.Verify(c => c.GetArtworkPreviews(2), Times.Once);
        }

        [Test]
        public async Task GetArtworkPreviewAsync_WhenOneClientReturnsEmpty_ReturnsPartialList()
        {
            // Arrange
            var clevelandArtworks = CreateClevelandArtPreviews(2);
            var chicagoArtworks = CreateChicagoArtPreviews(0);

            _mockClevelandClient.Setup(c => c.GetArtworkPreviews(2)).ReturnsAsync(clevelandArtworks);
            _mockChicagoClient.Setup(c => c.GetArtworkPreviews(2)).ReturnsAsync(chicagoArtworks);
            SetupMapper(clevelandArtworks, chicagoArtworks);

            // Act
            var result = await _artworksRepository.GetArtworkPreviewsAsync(2);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().HaveCount(2);
            _mockClevelandClient.Verify(c => c.GetArtworkPreviews(2), Times.Once);
            _mockChicagoClient.Verify(c => c.GetArtworkPreviews(2), Times.Once);
        }

        [Test]
        public async Task GetArtworkPreviewAsync_WhenClientsReturnItemsWithNullImage_FiltersThemOut()
        {
            // Arrange
            var clevelandArtworkWithoutImage = new ClevelandArtworkPreview { Id = 2, Images = new() { Web = null } };
            var clevelandArtworks = new List<ClevelandArtworkPreview> { clevelandArtworkWithoutImage };

            var chicagoArtworksWithImage = new ChicagoArtworkPreview { Id = 3, ImageId = "chicago_img_1" };
            var chicagoArtworks = new List<ChicagoArtworkPreview> { chicagoArtworksWithImage };

            _mockClevelandClient.Setup(c => c.GetArtworkPreviews(1)).ReturnsAsync(clevelandArtworks);
            _mockChicagoClient.Setup(c => c.GetArtworkPreviews(1)).ReturnsAsync(chicagoArtworks);

            _mockMapper.Setup(m => m.FromClevelandPreview(clevelandArtworkWithoutImage)).Returns(new ArtworkPreview { SourceId = 2, Thumbnail = null });
            _mockMapper.Setup(m => m.FromChicagoPreview(chicagoArtworksWithImage)).Returns(new ArtworkPreview { SourceId = 3, Thumbnail = new Image() });

            // Act
            var result = await _artworksRepository.GetArtworkPreviewsAsync(1);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().HaveCount(1);
            result.Value.Should().NotContain(a => a.SourceId == 2);
        }

        [Test]
        public async Task GetArtworkPreviewAsync_WhenOneClientThrowsException_ReturnsFailure()
        {
            //// Arrange
            //var expectedError = "Cleveland client failed.";
            //var exception = new HttpRequestException(expectedError, null, HttpStatusCode.InternalServerError);
            //_mockClevelandClient.Setup(c => c.GetArtworkPreview(5)).ThrowsAsync(exception);
            //_mockChicagoClient.Setup(c => c.GetArtworkPreviews(5)).ReturnsAsync(new List<ChicagoArtworkPreviewDto>());

            //// Act

            //// Assert

        }
    }
}
