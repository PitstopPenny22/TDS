using Data.Interfaces.Utils;
using System;
using System.Configuration;
using System.Net;

namespace Data.Utils
{
    public class OpenWeatherMapApiConsumer : IOpenWeatherMapApiConsumer
    {

        private readonly string ApiKey = ConfigurationManager.AppSettings[Shared.AppUtils.AppSettings.OpenWeatherMapApiKey.ToString()];
        private readonly IWebClientConsumer _webClientConsumer;

        public OpenWeatherMapApiConsumer(IWebClientConsumer webClientConsumer)
        {
            _webClientConsumer = webClientConsumer;
        }
     
        /// <summary>
        /// Gets the whole weather information as a string for the specified location, in metric units (i.e. temperature will be in degrees Celsius).
        /// </summary>
        public void GetWeatherInfoAsync(string location, DownloadStringCompletedEventHandler eventHandler)
        {
            var url = $"http://api.openweathermap.org/data/2.5/weather?q={location}&mode=xml&units=metric&appid={ApiKey}";
            _webClientConsumer.DownloadStringAsync(new Uri(url), eventHandler);
        }
    }
}