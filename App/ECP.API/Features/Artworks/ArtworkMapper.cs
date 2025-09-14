using ECP.API.Features.Artworks.Clients.ChicagoArtInstitute.Models;
using ECP.API.Features.Artworks.Clients.ClevelandMuseum;
using ECP.API.Features.Artworks.Clients.ClevelandMuseum.Models;
using ECP.Shared;
using System.Text.RegularExpressions;

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
            ArtworkType artworkType = ArtworkType.Undefined;
            Enum.TryParse(clevelandArtwork.Type, true, out artworkType);

            List<Artist?>? artworkArtists = clevelandArtwork.Creators?.Any() == true ? clevelandArtwork.Creators.Select(a => (a != null && a.Description.Contains('(')) ? new Artist()
            {
                Name = a.Description.Substring(0, a.Description.IndexOf('(')).Trim()
            } : null).ToList() : null;

            return new Artwork()
            {
                Id = string.Concat("cleveland_", clevelandArtwork.Id),
                Source = ArtworkSource.CLEVELAND_MUSEUM,
                SourceId = clevelandArtwork.Id,

                Title = clevelandArtwork.Title,
                Artists = artworkArtists ?? new(),

                DateDisplay = clevelandArtwork.CreationDateDisplay,
                DateStart = clevelandArtwork.DateStart,
                DateEnd = clevelandArtwork.DateEnd,
                SortableYear = clevelandArtwork.SortableYear,

                Type = artworkType,
                TypeDisplay = clevelandArtwork.Type,
                Classifications = BuildClassificationsFromCleveland(clevelandArtwork),
                Categories = string.IsNullOrEmpty(clevelandArtwork.Department) ? new() : new() { clevelandArtwork.Department },

                Materials = ExtractMaterialsFromCleveland(clevelandArtwork),
                MediumDisplay = CapitalizeFirstChar(clevelandArtwork.Technique),
                Techniques = new List<string>() { clevelandArtwork.Technique },

                Subjects = new(),
                Styles = new(),

                Culture = clevelandArtwork.Cultures ?? new(),
                PlaceOfOrigin = clevelandArtwork.Cultures.FirstOrDefault(),
                Dimensions = new Dimensions()
                {
                    Width = Math.Round((clevelandArtwork.Dimensions.Unframed?.Width ?? 0) * 100, 2),
                    Height = Math.Round((clevelandArtwork.Dimensions.Unframed?.Height ?? 0) * 100, 2)
                },
                DimensionsDisplay = ExtractCentimeter(clevelandArtwork.Measurements),
                Description = clevelandArtwork.Description,
                LongDescription = null,
                Keywords = null,
                Period = null,
                Dynasty = null,
                School = null,
                Movement = null,
                CurrentLocation = null,
                Department = null,
                Gallery = null,
                Images = ClevelandImageHelper.ConvertToImages(clevelandArtwork),
                SourceUrl = clevelandArtwork.Url,
                RelatedUrls = null,
                WikipediaUrl = null,
                RelatedArtworkIds = null,
                SeriesTitle = null,
                SeriesNumber = null
            };
        }
        public Artwork FromChicago(ChicagoArtwork chicagoArtwork)
        {
            ArtworkType artworkType = ArtworkType.Undefined;
            Enum.TryParse(chicagoArtwork.ArtworkTypeTitle, true, out artworkType);

            return new Artwork()
            {
                // Internal Use Properties
                Id = $"chicago_{chicagoArtwork.Id}",
                Source = ArtworkSource.CHICAGO_ART_INSTITUTE,
                SourceId = chicagoArtwork.Id,

                // Basic Information
                Title = chicagoArtwork.Title ?? string.Empty,
                Artists = chicagoArtwork.ArtistTitles?.Select(name => new Artist
                {
                    Name = name,
                }).ToList(),

                // Date properties
                DateDisplay = chicagoArtwork.DateDisplay,
                DateStart = chicagoArtwork.DateStart,
                DateEnd = chicagoArtwork.DateEnd,
                SortableYear = chicagoArtwork.DateStart ?? chicagoArtwork.DateEnd,

                // Type properties
                Type = artworkType,
                TypeDisplay = chicagoArtwork.ArtworkTypeTitle ?? string.Empty,
                Classifications = chicagoArtwork.ClassificationTitles ?? new List<string>(),
                Categories = chicagoArtwork.CategoryTitles ?? new List<string>(),

                // Material Properties
                Materials = chicagoArtwork.MaterialTitles ?? new List<string>(),
                MediumDisplay = CapitalizeFirstChar(chicagoArtwork.MediumDisplay),
                Techniques = chicagoArtwork.TechniqueTitles ?? new List<string>(),

                // Subject and style properties
                Subjects = chicagoArtwork.SubjectTitles ?? new List<string>(),
                Styles = chicagoArtwork.StyleTitles ?? new List<string>(),

                // Cultural properties
                Culture = new List<string>(), // Not available in Chicago API
                PlaceOfOrigin = chicagoArtwork.PlaceOfOrigin,

                // Physical properties
                Dimensions = chicagoArtwork.Dimensions,

                // Content and description
                Description = chicagoArtwork.Description,
                LongDescription = null, // Could combine description with other fields if needed
                Keywords = CombineKeywordsFromChicago(chicagoArtwork.Terms, chicagoArtwork.Themes),

                // Historical context - Not available in Chicago API
                Period = null,
                Dynasty = null,
                School = null,
                Movement = null,

                // Location
                CurrentLocation = null, // Not available in this API response
                Department = chicagoArtwork.Department,
                Gallery = chicagoArtwork.Gallery,

                // Digital assets and links
                Images = CreateImagesFromChicago(chicagoArtwork.Thumbnail, chicagoArtwork.ImageId),
                SourceUrl = chicagoArtwork.SourceUrl,
                RelatedUrls = new List<string>(),
                WikipediaUrl = null,

                // Relationships - Not available in Chicago API
                RelatedArtworkIds = new List<string>(),
                SeriesTitle = null,
                SeriesNumber = null
            };
        }
        public ArtworkPreview FromClevelandPreview(ClevelandArtworkPreview clevelandArtworkPreview)
        {
            ArtworkType artworkType = ArtworkType.Undefined;
            Enum.TryParse(clevelandArtworkPreview.Type, true, out artworkType);

            List<Artist?>? artworkArtists = clevelandArtworkPreview.Creators?.Any() == true ? clevelandArtworkPreview.Creators.Select(a => (a != null && a.Description.Contains('(')) ? new Artist()
            {
                Name = a.Description.Substring(0, a.Description.IndexOf('(')).Trim()
            } : null).ToList() : null;

            return new ArtworkPreview()
            {
                Id = string.Concat("cleveland_", clevelandArtworkPreview.Id),
                Source = ArtworkSource.CLEVELAND_MUSEUM,
                SourceId = clevelandArtworkPreview.Id,

                Title = clevelandArtworkPreview.Title,
                Artists = artworkArtists,
                Thumbnail = ClevelandImageHelper.GetSmallestImage(clevelandArtworkPreview),

                DateDisplay = clevelandArtworkPreview.CreationDateDisplay,
                DateStart = clevelandArtworkPreview.DateStart,
                DateEnd = clevelandArtworkPreview.DateEnd,
                SortableYear = clevelandArtworkPreview.SortableYear,

                Type = artworkType,
                TypeDisplay = clevelandArtworkPreview.Type,
                Classifications = BuildClassificationsFromCleveland(clevelandArtworkPreview),
                Categories = string.IsNullOrEmpty(clevelandArtworkPreview.Department) ? new() : new() { clevelandArtworkPreview.Department },

                Materials = ExtractMaterialsFromCleveland(clevelandArtworkPreview),
                MediumDisplay = clevelandArtworkPreview.Technique,
                Techniques = new List<string>() { clevelandArtworkPreview.Technique },

                Subjects = new(),
                Styles = new(),

                Culture = clevelandArtworkPreview.Cultures ?? new(),
                PlaceOfOrigin = clevelandArtworkPreview.Cultures.FirstOrDefault()
            };

        }
        public ArtworkPreview FromChicagoPreview(ChicagoArtworkPreview chicagoArtworkPreview)
        {
            ArtworkType artworkType = ArtworkType.Undefined;
            Enum.TryParse(chicagoArtworkPreview.ArtworkTypeTitle, true, out artworkType);

            List<Artist> artworkArtists = null;

            if (chicagoArtworkPreview.ArtistTitles != null && chicagoArtworkPreview.ArtistTitles.Count > 0)
            {
                artworkArtists = chicagoArtworkPreview.ArtistTitles.Select(a => new Artist()
                {
                    Name = a
                }).ToList();
            }

            Image artworkWebImage = null;

            if (!string.IsNullOrEmpty(chicagoArtworkPreview.ImageId))
            {
                artworkWebImage = new Image
                {
                    Url = $"https://www.artic.edu/iiif/2/{chicagoArtworkPreview.ImageId}/full/400,/0/default.jpg",
                    Width = chicagoArtworkPreview.Thumbnail?.Width ?? -1,
                    Height = chicagoArtworkPreview.Thumbnail?.Height ?? -1
                };
            }

            return new ArtworkPreview()
            {
                Id = string.Concat("chicago_", chicagoArtworkPreview.Id),
                Source = ArtworkSource.CHICAGO_ART_INSTITUTE,
                SourceId = chicagoArtworkPreview.Id,

                Title = chicagoArtworkPreview.Title,
                Artists = artworkArtists,
                Thumbnail = artworkWebImage,

                DateDisplay = chicagoArtworkPreview.DateDisplay,
                DateStart = chicagoArtworkPreview.DateStart,
                DateEnd = chicagoArtworkPreview.DateEnd,
                SortableYear = chicagoArtworkPreview.DateStart,

                Type = artworkType,
                TypeDisplay = chicagoArtworkPreview.ArtworkTypeTitle ?? "Unknown",
                Classifications = chicagoArtworkPreview.ClassificationTitles ?? new List<string>(),
                Categories = chicagoArtworkPreview.CategoryTitles ?? new(),

                Materials = chicagoArtworkPreview.MaterialTitles ?? new List<string>(),
                MediumDisplay = chicagoArtworkPreview.MediumDisplay,
                Techniques = chicagoArtworkPreview.TechniqueTitles ?? new List<string>(),

                Subjects = chicagoArtworkPreview.SubjectTitles ?? new(),
                Styles = chicagoArtworkPreview.StyleTitles ?? new List<string>(),

                Culture = string.IsNullOrEmpty(chicagoArtworkPreview.PlaceOfOrigin) ? new() : new() { chicagoArtworkPreview.PlaceOfOrigin },
                PlaceOfOrigin = chicagoArtworkPreview.PlaceOfOrigin
            };

        }

        private static List<string> BuildClassificationsFromCleveland(ClevelandArtworkPreview artworkPreview)
        {
            var classifications = new List<string>();

            if (!string.IsNullOrEmpty(artworkPreview.Type))
                classifications.Add(artworkPreview.Type);

            if (artworkPreview.Cultures?.Any() == true)
                classifications.AddRange(artworkPreview.Cultures);

            if (!string.IsNullOrEmpty(artworkPreview.Technique))
                classifications.Add(artworkPreview.Technique);

            if (!string.IsNullOrEmpty(artworkPreview.Department))
                classifications.Add(artworkPreview.Department);

            if (!string.IsNullOrEmpty(artworkPreview.Collection))
                classifications.Add(artworkPreview.Collection);

            return classifications.Distinct().ToList();

        }

        private static List<string> ExtractMaterialsFromCleveland(ClevelandArtworkPreview artworkPreview)
        {
            var materials = new List<string>();

            if (!string.IsNullOrEmpty(artworkPreview.Technique))
            {
                var parts = artworkPreview.Technique.Split(new[] { " on ", " and ", " with ", "," },
                    StringSplitOptions.RemoveEmptyEntries);
                materials.AddRange(parts.Select(p => p.Trim()));
            }

            if (artworkPreview.Materials != null && artworkPreview.Materials.Count != 0)
            {
                foreach (var material in artworkPreview.Materials)
                {
                    materials.Add(material.Description);
                }
            }

            return materials.Where(m => !string.IsNullOrEmpty(m)).Distinct().ToList();
        }

        private List<string> CombineKeywordsFromChicago(List<string>? terms, List<string>? themes)
        {
            var keywords = new List<string>();

            if (terms != null)
                keywords.AddRange(terms);

            if (themes != null)
                keywords.AddRange(themes);

            return keywords.Distinct().ToList();
        }

        private Images CreateImagesFromChicago(Thumbnail? thumbnail, string? imageId)
        {
            var images = new Images();

            if (!string.IsNullOrEmpty(imageId))
            {
                images.Web = new Image
                {
                    Url = $"https://www.artic.edu/iiif/2/{imageId}/full/843,/0/default.jpg",
                };

                images.Full = new Image
                {
                    Url = $"https://www.artic.edu/iiif/2/{imageId}/full/1686,/0/default.jpg",
                };
            }
            else
            {
                images.Web = new Image { Url = string.Empty };
            }

            return images;
        }

        private string? CapitalizeFirstChar(string? input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            if (input.Length == 1)
                return input.ToUpper();

            return char.ToUpper(input[0]) + input.Substring(1);
        }

        private string ExtractCentimeter(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }


            var regex = new Regex(@"(\d+\.?\d*\s*x\s*\d+\.?\d*\s*cm)");

            var match = regex.Match(input);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return input;
        }
    }

}
