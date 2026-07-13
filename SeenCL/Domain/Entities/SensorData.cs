using System;

namespace SeenCL.Domain.Entities
{
    public class SensorData
    {
        public long DataID { get; set; }
        public double Reader { get; set; }
        public DateTime Timestamp { get; set; }
        public int SensorID { get; set; }
        public int DeviceID { get; set; }
    }
}
