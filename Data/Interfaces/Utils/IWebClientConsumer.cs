using System.Net;

namespace Data.Interfaces.Utils
{
    public interface IWebClientConsumer
    {
        void DownloadStringAsync(System.Uri url, DownloadStringCompletedEventHandler eventHandler);
    }
}