using System.Net;

namespace Data.Interfaces.Utils
{
    public interface IOpenWeatherMapApiConsumer
    {
        void GetWeatherInfoAsync(string location, DownloadStringCompletedEventHandler eventHandler);
    }
}