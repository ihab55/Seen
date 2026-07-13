using System;

namespace SeenCL.Domain.Entities
{
    public class Alert
    {
        public int AlertID { get; set; }
        public int SensorID { get; set; }
        public string AlertType { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int DeviceID { get; set; }
    }
}
