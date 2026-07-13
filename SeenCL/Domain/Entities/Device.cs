using System;

namespace SeenCL.Domain.Entities
{
    public class Device
    {
        public int DeviceID { get; set; }
        public string DeviceName { get; set; } = string.Empty;
        public string DeviceType { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string MacAddress { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime? RegisteredAt { get; set; }
    }
}
