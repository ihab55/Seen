namespace SeenCL.DTOs
{
    public class SensorDTO
    {
        public int SensorID { get; set; }
        public string SensorName { get; set; }
        public string SensorType { get; set; }
        public string Unit { get; set; }
        public double MinSafeValue { get; set; }
        public double MaxSafeValue { get; set; }
        public string? Description { get; set; }

        public SensorDTO(int SensorID, string SensorName, string SensorType, string Unit, double MinSafeValue,
            double MaxSafeValue, string? Description)
        {
            this.SensorID = SensorID;
            this.SensorName = SensorName;
            this.SensorType = SensorType;
            this.Unit = Unit;
            this.MinSafeValue = MinSafeValue;
            this.MaxSafeValue = MaxSafeValue;
            this.Description = Description;
        }
    }
}
