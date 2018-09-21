using Data.Interfaces.Utils;
using System.Net;

namespace Data.Utils
{
    public class WebClientConsumer : IWebClientConsumer
    {
        public void DownloadStringAsync(System.Uri url, DownloadStringCompletedEventHandler webClient_DownloadStringCompleted)
        {
            using (var webClient = new WebClient())
            {
                webClient.DownloadStringCompleted += webClient_DownloadStringCompleted;
                webClient.DownloadStringAsync(url);
            }
        }
    }
}