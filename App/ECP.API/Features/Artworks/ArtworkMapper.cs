using ECP.API.Features.Artworks.Clients.ChicagoArtInstitute.Models;
using ECP.API.Features.Artworks.Clients.ClevelandMuseum.Models;
using ECP.API.Features.Artworks.Models;

namespace ECP.API.Features.Artworks
{
    public interface IArtworkMapper
    {
        Artwork FromCleveland(ClevelandMuseumArtwork clevelandArtwork);

        ArtworkPreview FromClevelandPreview(ClevelandMuseumArtworkPreview clevelandArtworkPreview);

        ArtworkPreview FromChicagoPreview(ChicagoInstArtPreview chicagoArtworkPreview);

    }
    public class ArtworkMapper : IArtworkMapper
    {
        public static int ParseOrDefault(string value, int defaultValue)
        {
            if (int.TryParse(value, out int result))
            {
                return result;
            }
            return defaultValue;
        }
        public Artwork FromCleveland(ClevelandMuseumArtwork clevelandArtwork)
        {

            string artworkId = string.Concat("cleveland_", clevelandArtwork.Id);
            var artworkSource = ArtworkSource.CLEVELAND_MUSEUM;
            int artworkSourceId = clevelandArtwork.Id;
            string title = clevelandArtwork.Title;
            string creationDate = clevelandArtwork.CreationDate;
            int minCreationYear = clevelandArtwork.CreationDateEarliest;
            int maxCreationYear = clevelandArtwork.CreationDateLatest;
            string medium = clevelandArtwork.Technique;
            string type = clevelandArtwork.Type;
            string description = clevelandArtwork.Description;
            List<string> culture = clevelandArtwork.Culture;
            string sourceUrl = clevelandArtwork.Url;

            var artists = clevelandArtwork.Creators?.Any() == true
                ? clevelandArtwork.Creators.Select(a => new Artist() { Name = a.Description }).ToList()
                : null;

            Dimensions dimensions = null;
            if (clevelandArtwork.Dimensions?.Unframed != null)
            {
                dimensions = new Dimensions()
                {
                    Width = clevelandArtwork.Dimensions.Unframed.Width,
                    Height = clevelandArtwork.Dimensions.Unframed.Height
                };
            }

            Image webImage = null;
            if (clevelandArtwork.Images?.Web != null)
            {
                webImage = new Image()
                {
                    Url = clevelandArtwork.Images.Web.Url,
                    Width = ArtworkMapper.ParseOrDefault(clevelandArtwork.Images.Web.Width, -1),
                    Height = ArtworkMapper.ParseOrDefault(clevelandArtwork.Images.Web.Height, -1)
                };
            }

            Image fullImage = null;
            if (clevelandArtwork.Images?.Full != null)
            {
                fullImage = new Image()
                {
                    Url = clevelandArtwork.Images.Full.Url,
                    Width = ArtworkMapper.ParseOrDefault(clevelandArtwork.Images.Full.Width, -1),
                    Height = ArtworkMapper.ParseOrDefault(clevelandArtwork.Images.Full.Height, -1)
                };
            }

            var images = new Images()
            {
                Thumbnail = null,
                Web = webImage,
                Full = fullImage
            };

            Artwork artwork = new Artwork()
            {
                Id = artworkId,
                Source = artworkSource,
                SourceId = artworkSourceId,
                Title = title,
                Artists = artists,
                CreationDate = creationDate,
                MinCreationYear = minCreationYear,
                MaxCreationYear = maxCreationYear,
                Medium = medium,
                Type = type,
                Dimensions = dimensions,
                Description = description,
                Culture = culture,
                SourceUrl = sourceUrl,
                Images = images
            };

            return artwork;
        }

        public ArtworkPreview FromClevelandPreview(ClevelandMuseumArtworkPreview clevelandArtworkPreview)
        {
            string artworkId = string.Concat("cleveland_", clevelandArtworkPreview.Id);
            int artworkSourceId = clevelandArtworkPreview.Id;
            string artworkTitle = clevelandArtworkPreview.Title;
            List<Artist?>? artworkArtists = clevelandArtworkPreview.Creators?.Any() == true ? clevelandArtworkPreview.Creators.Select(a => (a != null && a.Description.Contains('(')) ? new Artist()
            {
                Name = a.Description.Substring(0, a.Description.IndexOf('(')).Trim()
            } : null).ToList() : null;

            Image artworkWebImage = null;
            if (clevelandArtworkPreview.Images.Web != null)
            {
                artworkWebImage = new Image
                {
                    Url = clevelandArtworkPreview.Images.Web.Url,
                    Width = ArtworkMapper.ParseOrDefault(clevelandArtworkPreview.Images.Web.Width, -1),
                    Height = ArtworkMapper.ParseOrDefault(clevelandArtworkPreview.Images.Web.Height, -1)
                };
            }

            return new ArtworkPreview()
            {
                Id = artworkId,
                Source = ArtworkSource.CLEVELAND_MUSEUM,
                SourceId = artworkSourceId,
                Title = artworkTitle,
                Artists = artworkArtists,
                WebImage = artworkWebImage
            };

        }
        public ArtworkPreview FromChicagoPreview(ChicagoInstArtPreview chicagoArtworkPreview)
        {
            string artworkId = string.Concat("chicago_", chicagoArtworkPreview.Id);
            int artworkSourceId = chicagoArtworkPreview.Id;
            string artworkTitle = chicagoArtworkPreview.Title;
            List<Artist> artworkArtists = null;

            if (chicagoArtworkPreview.Artists != null && chicagoArtworkPreview.Artists.Count > 0)
            {
                artworkArtists = chicagoArtworkPreview.Artists.Select(a => new Artist()
                {
                    Name = a
                }).ToList();
            }

            Image artworkWebImage = null;

            if (!string.IsNullOrEmpty(chicagoArtworkPreview.ImageId))
            {
                artworkWebImage = new Image
                {
                    Url = $"https://www.artic.edu/iiif/2/{chicagoArtworkPreview.ImageId}/full/843,/0/default.jpg",
                    Width = chicagoArtworkPreview.ThumbnailImage?.Width ?? -1,
                    Height = chicagoArtworkPreview.ThumbnailImage?.Height ?? -1
                };
            }

            return new ArtworkPreview()
            {
                Id = artworkId,
                Source = ArtworkSource.CHICAGO_ART_INSTITUTE,
                SourceId = artworkSourceId,
                Title = artworkTitle,
                Artists = artworkArtists,
                WebImage = artworkWebImage
            };

        }

    }


}
