using SeenCL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeenCL.Services
{
    public interface IDeviceService
    {
        Task<int> RegisterDeviceAsync(DeviceDTO dto);
        Task<DeviceDTO?> GetDeviceBySerialAsync(string serialNumber);
        Task<bool> ToggleDeviceStatusAsync(int deviceId, bool isActive);
        Task<IEnumerable<DeviceDTO>> GetAllDevicesAsync();
    }
}
