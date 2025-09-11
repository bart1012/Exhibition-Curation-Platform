using ECP.API.Features.Artworks;
using ECP.Shared;

namespace ECP.API.Tests.UnitTests.Features.Artworks.ServiceHelpers
{
    internal class TestableArtworksService : ArtworksService
    {
        public TestableArtworksService() : base(null, null)
        {
        }

        public new Result<List<ArtworkPreview>> Sort(List<ArtworkPreview> list, string sortQuery)
        {
            return base.Sort(list, sortQuery);
        }

        public new Result<(string, char)> ParseAndValidateSortField(string sortQuery)
        {
            return base.ParseAndValidateSortQuery(sortQuery);
        }

        public new Result<List<ArtworkPreview>> Filter(List<ArtworkPreview> list, List<string> options)
        {
            return base.Filter(list, options);
        }

        public new Result<Dictionary<string, List<string>>> ParseAndValidateFilters(List<string>? filterQueries)
        {
            return base.ParseAndValidateFilters(filterQueries);
        }

    }
}
