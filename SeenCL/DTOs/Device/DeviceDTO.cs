namespace SeenCL.DTOs
{
    public class DeviceDTO
    {
        public int DeviceID { get; set; }
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
        public string SerialNumber { get; set; }
        public string MacAddress { get; set; }
        public bool IsActive { get; set; }
        public DateTime? RegisteredAt { get; set; }
        public DeviceDTO(int DeviceID, string DeviceName, string DeviceType,
            string SerialNumber, string MacAddress, bool IsActive, DateTime? RegisteredAt)
        {
            this.DeviceID = DeviceID;
            this.DeviceName = DeviceName;
            this.DeviceType = DeviceType;
            this.SerialNumber = SerialNumber;
            this.MacAddress = MacAddress;
            this.IsActive = IsActive;
            this.RegisteredAt = RegisteredAt;
        }
    }
}
