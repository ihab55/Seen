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
    public class AlertService : IAlertService
    {
        private readonly IAlertRepository _alertRepository;

        public AlertService(IAlertRepository alertRepository)
        {
            _alertRepository = alertRepository;
        }

        public async Task<IEnumerable<AlertDTO>> GetAlertsByDeviceAsync(int deviceId)
        {
            var alerts = await Task.FromResult(_alertRepository.GetByDeviceId(deviceId));
            return alerts.Select(a => new AlertDTO(
                a.AlertID,
                a.SensorID,
                a.AlertType,
                a.Message,
                a.CreatedAt,
                a.DeviceID
            ));
        }

        public async Task<int> CreateAlertAsync(AlertDTO dto)
        {
            var alert = new Alert
            {
                SensorID = dto.SensorID,
                AlertType = dto.AlertType,
                Message = dto.Message,
                CreatedAt = DateTime.UtcNow,
                DeviceID = dto.DeviceID
            };
            return await Task.FromResult(_alertRepository.Create(alert));
        }
    }
}
