namespace Data.CustomEventArgs
{
    public class TemperatureUpdatedEventArgs : System.EventArgs
    {
        public double NewTemperature { get; set; }
    }
}
