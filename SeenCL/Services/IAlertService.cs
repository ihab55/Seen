using SeenCL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeenCL.Services
{
    public interface IAlertService
    {
        Task<IEnumerable<AlertDTO>> GetAlertsByDeviceAsync(int deviceId);
        Task<int> CreateAlertAsync(AlertDTO dto);
    }
}
