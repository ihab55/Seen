namespace SeenCL.DTOs
{
    public class DeviceResponseDTO
    {
        public int DeviceID { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public string DeviceModel { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string? FirmwareVersion { get; set; }
        public DateTime LastConnection { get; set; }
    }

    public class DeviceRegistrationDTO
    {
        public string SerialNumber { get; set; } = string.Empty;
        public string DeviceModel { get; set; } = string.Empty;
        public string? FirmwareVersion { get; set; }
    }
}
