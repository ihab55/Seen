namespace SeenCL.DTOs
{
    public class SensorDataDTO
    {
        public long DataID { get; set; }
        public double Reader { get; set; }
        public DateTime Timestamp { get; set; }
        public int SensorID { get; set; }
        public int DeviceID { get; set; }

        // ????? ?????? ????? (Optional)
        public string? SensorName { get; set; }
        public string? Unit { get; set; }

        public SensorDataDTO(long DataID, double Reader, DateTime Timestamp, int SensorID, int DeviceID,
            string? SensorName = null, string? Unit = null)
        {
            this.DataID = DataID;
            this.Reader = Reader;
            this.Timestamp = Timestamp;
            this.SensorID = SensorID;
            this.DeviceID = DeviceID;
            this.SensorName = SensorName;
            this.Unit = Unit;
        }
    }
}

