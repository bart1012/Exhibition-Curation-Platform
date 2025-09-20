using ECP.UI.Server.Models;

namespace ECP.UI.Server.Services
{
    public interface IBackgroundService
    {
        BackgroundData GetBackgroundImage();
    }
    public class BackgroundService : IBackgroundService
    {
        private readonly List<BackgroundData> _images = new()
        {
            new BackgroundData()
            {
                ImgUrl = "https://openaccess-cdn.clevelandart.org/1981.18/1981.18_web.jpg",
                Title = "The Holy Family on the Steps",
                Artist = "Nicolas Poussin (French, 1594–1665)"
            },
            new BackgroundData()
            {
                ImgUrl = "https://openaccess-cdn.clevelandart.org/1951.355/1951.355_web.jpg",
                Title = "Portrait of a Family Playing Music",
                Artist = "Pieter de Hooch (Dutch, 1629–1684)"
            },
            new BackgroundData()
            {
                ImgUrl = "https://openaccess-cdn.clevelandart.org/1965.233/1965.233_web.jpg",
                Title = "Twilight in the Wilderness",
                Artist = "Frederic Edwin Church (American, 1826–1900)"
            }
        };

        private readonly Random _random = new();

        public BackgroundData GetBackgroundImage()
        {
            return _images[_random.Next(0, _images.Count)];
        }
    }
}
