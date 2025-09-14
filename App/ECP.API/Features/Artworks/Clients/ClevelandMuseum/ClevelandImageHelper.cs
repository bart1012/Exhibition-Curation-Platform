using ECP.API.Features.Artworks.Clients.ClevelandMuseum.Models;
using ECP.Shared;

namespace ECP.API.Features.Artworks.Clients.ClevelandMuseum
{
    public static class ClevelandImageHelper
    {
        public static Image? GetSmallestImage(ClevelandArtworkPreview artwork)
        {
            if (artwork.AltImages?.Any() == true)
            {
                var smallestAltImage = GetSmallestImageFromCollection(artwork.AltImages);
                if (smallestAltImage != null)
                    return ConvertToImage(smallestAltImage);
            }

            return ConvertToImage(artwork.Images?.Web);
        }

        public static Images? ConvertToImages(ClevelandArtwork artwork)
        {
            Images images = new();
            if (artwork.Images != null)
            {
                if (artwork.Images.Web != null)
                {
                    images.Web = ConvertToImage(artwork.Images.Web);
                }
                if (artwork.Images.Full != null)
                {
                    images.Full = ConvertToImage(artwork.Images.Full);

                }
            }

            return images;
        }

        public static Image? ConvertToImage(ImageData? imageData)
        {
            if (imageData == null)
                return null;

            return new Image
            {
                Url = imageData.Url ?? string.Empty,
                Width = TryParseInt(imageData.Width),
                Height = TryParseInt(imageData.Height)
            };
        }


        private static int TryParseInt(string? value)
        {
            if (int.TryParse(value, out int result))
                return result;
            return 0;
        }

        private static ImageData? GetSmallestImageFromCollection(List<ClevelandMuseumImages> imageCollection)
        {
            var allImages = new List<ImageData>();

            foreach (var imageSet in imageCollection)
            {
                if (imageSet.Web != null) allImages.Add(imageSet.Web);
                if (imageSet.Print != null) allImages.Add(imageSet.Print);
                if (imageSet.Full != null) allImages.Add(imageSet.Full);
            }

            if (!allImages.Any())
                return null;

            return allImages
                .Where(img => img != null)
                .OrderBy(img => GetFilesize(img.Filesize))
                .ThenBy(img => GetPixelArea(img.Width, img.Height))
                .FirstOrDefault();
        }

        private static long GetFilesize(string? filesizeString)
        {
            if (string.IsNullOrEmpty(filesizeString))
                return long.MaxValue;

            if (long.TryParse(filesizeString.Trim(), out long filesize))
                return filesize;

            var cleanSize = filesizeString.ToUpper().Replace("B", "").Trim();

            if (cleanSize.EndsWith("K"))
            {
                if (double.TryParse(cleanSize.Replace("K", ""), out double kb))
                    return (long)(kb * 1024);
            }
            else if (cleanSize.EndsWith("M"))
            {
                if (double.TryParse(cleanSize.Replace("M", ""), out double mb))
                    return (long)(mb * 1024 * 1024);
            }

            return long.MaxValue;
        }

        private static long GetPixelArea(string? widthString, string? heightString)
        {
            if (string.IsNullOrEmpty(widthString) || string.IsNullOrEmpty(heightString))
                return long.MaxValue;

            if (int.TryParse(widthString, out int width) &&
                int.TryParse(heightString, out int height))
            {
                return (long)width * height;
            }

            return long.MaxValue;
        }

        public static ImageData? GetSmallestImageWithCriteria(ClevelandArtworkPreview artwork,
            int maxWidth = int.MaxValue,
            long maxFilesize = long.MaxValue)
        {
            var candidates = new List<ImageData>();

            if (artwork.AltImages?.Any() == true)
            {
                foreach (var imageSet in artwork.AltImages)
                {
                    if (imageSet.Web != null) candidates.Add(imageSet.Web);
                    if (imageSet.Print != null) candidates.Add(imageSet.Print);
                    if (imageSet.Full != null) candidates.Add(imageSet.Full);
                }
            }

            if (artwork.Images?.Web != null)
                candidates.Add(artwork.Images.Web);

            if (!candidates.Any())
                return null;

            return candidates
                .Where(img => GetPixelWidth(img.Width) <= maxWidth &&
                             GetFilesize(img.Filesize) <= maxFilesize)
                .OrderBy(img => GetFilesize(img.Filesize))
                .ThenBy(img => GetPixelArea(img.Width, img.Height))
                .FirstOrDefault()
                ?? candidates.OrderBy(img => GetFilesize(img.Filesize)).First();
        }

        private static int GetPixelWidth(string? widthString)
        {
            if (int.TryParse(widthString, out int width))
                return width;
            return int.MaxValue;
        }
    }
}
