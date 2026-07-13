using SeenCL.Domain.Entities;
using SeenCL.DTOs;
using SeenCL.Repositories;
using SeenCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeenBLL.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _repository;

        public DeviceService(IDeviceRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> RegisterDeviceAsync(DeviceDTO dto)
        {
            var device = new Device
            {
                DeviceName = dto.DeviceName,
                DeviceType = dto.DeviceType,
                SerialNumber = dto.SerialNumber,
                MacAddress = dto.MacAddress,
                IsActive = true,
                RegisteredAt = DateTime.UtcNow
            };
            return await Task.FromResult(_repository.Create(device));
        }

        public async Task<DeviceDTO?> GetDeviceBySerialAsync(string serialNumber)
        {
            var devices = await Task.FromResult(_repository.GetAll());
            var device = devices.FirstOrDefault(d => d.SerialNumber == serialNumber);
            return device != null ? MapToDTO(device) : null;
        }

        public async Task<bool> ToggleDeviceStatusAsync(int deviceId, bool isActive)
        {
            var device = await Task.FromResult(_repository.GetById(deviceId));
            if (device == null) return false;
            device.IsActive = isActive;
            return await Task.FromResult(_repository.Update(device));
        }

        public async Task<IEnumerable<DeviceDTO>> GetAllDevicesAsync()
        {
            var devices = await Task.FromResult(_repository.GetAll());
            return devices.Select(MapToDTO);
        }

        private DeviceDTO MapToDTO(Device d)
        {
            return new DeviceDTO(
                d.DeviceID,
                d.DeviceName,
                d.DeviceType,
                d.SerialNumber,
                d.MacAddress,
                d.IsActive,
                d.RegisteredAt
            );
        }
    }
}
