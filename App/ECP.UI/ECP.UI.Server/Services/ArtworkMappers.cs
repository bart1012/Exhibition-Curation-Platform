using ECP.Shared;

namespace ECP.UI.Server.Services
{
    public static class ArtworkMappers
    {
        public static ArtworkPreview ToArtworkPreview(this Artwork artwork)
        {
            return new ArtworkPreview
            {
                // Internal Use Properties
                Id = artwork.Id,
                Source = artwork.Source,
                SourceId = artwork.SourceId,

                // Basic Information
                Title = artwork.Title,
                Artists = artwork.Artists,
                Thumbnail = artwork.Images?.Web,

                // Date Properties
                DateDisplay = artwork.DateDisplay,
                DateStart = artwork.DateStart,
                DateEnd = artwork.DateEnd,
                SortableYear = artwork.SortableYear,

                // Type Properties
                Type = artwork.Type,
                TypeDisplay = artwork.TypeDisplay,
                Classifications = artwork.Classifications,
                Categories = artwork.Categories,

                // Material Properties
                Materials = artwork.Materials ?? new List<string>(),
                MediumDisplay = artwork.MediumDisplay ?? string.Empty,
                Techniques = artwork.Techniques,

                // Subject and Style Properties
                Subjects = artwork.Subjects ?? new List<string>(),
                Styles = artwork.Styles,

                // Cultural Properties
                Culture = artwork.Culture,
                PlaceOfOrigin = artwork.PlaceOfOrigin
            };
        }
    }
}
