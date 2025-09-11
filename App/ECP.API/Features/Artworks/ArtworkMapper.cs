using ECP.API.Features.Artworks.Clients.ChicagoArtInstitute.Models;
using ECP.API.Features.Artworks.Clients.ClevelandMuseum.Models;
using ECP.Shared;

namespace ECP.API.Features.Artworks
{
    public interface IArtworkMapper
    {
        Artwork FromCleveland(ClevelandArtwork clevelandArtwork);
        Artwork FromChicago(ChicagoArtwork chicagoArtwork);
        ArtworkPreview FromClevelandPreview(ClevelandArtworkPreview clevelandArtworkPreview);

        ArtworkPreview FromChicagoPreview(ChicagoArtworkPreview chicagoArtworkPreview);

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
        public Artwork FromCleveland(ClevelandArtwork clevelandArtwork)
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
            ArtworkType artworkType = ArtworkType.Undefined;
            Enum.TryParse(clevelandArtwork.Type, true, out artworkType);

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
                Type = artworkType,
                Dimensions = dimensions,
                Description = description,
                Culture = culture,
                SourceUrl = sourceUrl,
                Images = images
            };

            return artwork;
        }
        public Artwork FromChicago(ChicagoArtwork chicagoArtwork)
        {

            string artworkId = string.Concat("chicago_", chicagoArtwork.Id);
            var artworkSource = ArtworkSource.CHICAGO_ART_INSTITUTE;
            int artworkSourceId = chicagoArtwork.Id;
            string title = chicagoArtwork.Title;
            string creationDate = chicagoArtwork.DateDisplay;
            int? minCreationYear = chicagoArtwork.EarliestCreationDate;
            int? maxCreationYear = chicagoArtwork.LatestCreationDate;
            string medium = chicagoArtwork.MediumDisplay;
            string type_display = chicagoArtwork.Type;
            string description = chicagoArtwork.Description;
            string placeOfOrigin = chicagoArtwork.PlaceOfOrigin;
            ArtworkType artworkType = ArtworkType.Undefined;
            Enum.TryParse(chicagoArtwork.Type, true, out artworkType);

            var artists = chicagoArtwork.Artists?.Any() == true
                ? chicagoArtwork.Artists.Select(a => new Artist() { Name = a }).ToList()
                : null;

            string dimensions = chicagoArtwork.Dimensions;




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
                TypeDisplay = chicagoArtwork.Type,
                Type = artworkType
            };

            return artwork;
        }
        public ArtworkPreview FromClevelandPreview(ClevelandArtworkPreview clevelandArtworkPreview)
        {
            string artworkId = string.Concat("cleveland_", clevelandArtworkPreview.Id);
            int artworkSourceId = clevelandArtworkPreview.Id;
            string artworkTitle = clevelandArtworkPreview.Title;
            int creationDate = 0;
            ArtworkType artworkType = ArtworkType.Undefined;
            Enum.TryParse(clevelandArtworkPreview.Type, true, out artworkType);

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
                ArtworkType = artworkType,
                ArtworkTypeDisplay = clevelandArtworkPreview.Type,
                Artists = artworkArtists,
                WebImage = artworkWebImage,
                CreationYear = int.TryParse(clevelandArtworkPreview.CreationDate, out creationDate) ? creationDate : -1
            };

        }
        public ArtworkPreview FromChicagoPreview(ChicagoArtworkPreview chicagoArtworkPreview)
        {
            string artworkId = string.Concat("chicago_", chicagoArtworkPreview.Id);
            int artworkSourceId = chicagoArtworkPreview.Id;
            string artworkTitle = chicagoArtworkPreview.Title;
            int? creationYear = 0;
            ArtworkType artworkType = ArtworkType.Undefined;
            Enum.TryParse(chicagoArtworkPreview.Type, true, out artworkType);

            if (chicagoArtworkPreview.EarliestCreationDate != 0 && chicagoArtworkPreview.LatestCreationDate != 0)
            {
                creationYear = (chicagoArtworkPreview.EarliestCreationDate + chicagoArtworkPreview.LatestCreationDate) / 2;
            }
            else
            {
                if (chicagoArtworkPreview.EarliestCreationDate == 0 && chicagoArtworkPreview.LatestCreationDate == 0)
                {
                    creationYear = -1;
                }
                else
                {
                    creationYear = chicagoArtworkPreview.EarliestCreationDate == 0 ? chicagoArtworkPreview.LatestCreationDate : chicagoArtworkPreview.EarliestCreationDate;

                }
            }

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
                ArtworkType = artworkType,
                ArtworkTypeDisplay = chicagoArtworkPreview.Type,
                Title = artworkTitle,
                CreationYear = creationYear,
                Artists = artworkArtists,
                WebImage = artworkWebImage
            };

        }

    }


}
