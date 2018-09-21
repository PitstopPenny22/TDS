using System;
using System.Diagnostics;
using System.Net;
using Data.CustomEventArgs;
using Data.Interfaces.Utils;

namespace Data.Utils
{
    public class WeatherService : IWeatherService
    {
        private readonly IOpenWeatherMapApiConsumer _weatherMapApiConsumer;
        public event EventHandler TemperatureUpdated;
        void OnTemperatureUpdated(double temperature)
        {
            TemperatureUpdated?.Invoke(null, new TemperatureUpdatedEventArgs { NewTemperature = temperature });
        }

        public WeatherService(IOpenWeatherMapApiConsumer weatherMapApiConsumer)
        {
            _weatherMapApiConsumer = weatherMapApiConsumer;
        }
        
        /* Sample data returned by OpenWeatherMap
        <current>
            <city id="2643743" name="London">
                <coord lon="-0.13" lat="51.51"/>
                <country>GB</country>
                <sun rise="2017-04-06T05:22:19" set="2017-04-06T18:43:57"/>
            </city>
            <temperature value="10.31" min="8" max="14" unit="metric"/>
            <humidity value="62" unit="%"/>
            <pressure value="1029" unit="hPa"/>
            <wind>
                <speed value="2.1" name="Light breeze"/>
                <gusts/>
                <direction value="300" code="WNW" name="West-northwest"/>
            </wind>
            <clouds value="0" name="clear sky"/>
            <visibility value="10000"/>
            <precipitation mode="no"/>
            <weather number="800" value="clear sky" icon="01n"/>
            <lastupdate value="2017-04-06T21:20:00"/>
        </current>
        */

        public void GetTemperatureForLocationAsync(string location, EventHandler temperatureUpdatedEvent)
        {
            TemperatureUpdated += temperatureUpdatedEvent;
            _weatherMapApiConsumer.GetWeatherInfoAsync(location, DownloadStringCompletedEventHandler);
        }

        private void DownloadStringCompletedEventHandler(object sender, DownloadStringCompletedEventArgs e)
        {
            if (!(sender is WebClient webClient))
            {
                return;
            }
            webClient.DownloadStringCompleted -= DownloadStringCompletedEventHandler;
            var content = e.Result;
            var tempAsString = ReadTemperature(content);
            var success = double.TryParse(tempAsString, out var temperature);
            if (!success)
            {
                Debug.WriteLine("CurrentSelectionViewModel Error | Cannot parse returned value '{0}' as double.", tempAsString);
            }
            OnTemperatureUpdated(temperature);
        }
        private static string ReadTemperature(string content)
        {
            // Get value from a line similar to this: <temperature value="10.31" min="8" max="14" unit="metric"/>
            const string temperatureSearchString = "<temperature";
            var temperatureIndex = content.IndexOf(temperatureSearchString, StringComparison.Ordinal);
            if (temperatureIndex == -1)
            {
                Debug.WriteLine("OpenWeatherMapApiConsumer.Instance Error | Can't find temperature.");
                return string.Empty;
            }
            var valueIndex = content.IndexOf("value=", temperatureIndex, StringComparison.Ordinal);
            if (valueIndex == -1)
            {
                Debug.WriteLine("OpenWeatherMapApiConsumer.Instance Error | Can't find temperature value.");
                return string.Empty;
            }
            var temperatureValueIndex = valueIndex + "value=".Length + 1; // + 1 is for quote
            return content.Substring(temperatureValueIndex, content.IndexOf("\"", temperatureValueIndex, StringComparison.Ordinal) - temperatureValueIndex);
        }
    }
}
