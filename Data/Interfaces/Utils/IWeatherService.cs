using System;

namespace Data.Interfaces.Utils
{
    public interface IWeatherService
    {
        event EventHandler TemperatureUpdated;
        void GetTemperatureForLocationAsync(string location, EventHandler temperatureUpdatedEvent);
    }
}