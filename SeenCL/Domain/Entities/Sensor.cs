namespace SeenCL.Domain.Entities
{
    public class Sensor
    {
        public int SensorID { get; set; }
        public string SensorName { get; set; } = string.Empty;
        public string SensorType { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public double MinSafeValue { get; set; }
        public double MaxSafeValue { get; set; }
        public string? Description { get; set; }
    }
}
