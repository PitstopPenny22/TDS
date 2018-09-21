namespace Data
{
    /// <summary>
    /// Struct that groups together details for a tyre as gathered from reading the XML file.
    /// </summary>
    public struct TyreDetails
    {
        public string Name { get; internal set; }
        public string Family { get; internal set; }
        public string Type { get; internal set; }
        public string Placement { get; internal set; }
        public double DegradationCoefficient { get; internal set; }
    }
}