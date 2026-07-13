using SeenCL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeenCL.Services
{
    public interface IAdminService
    {
        Task<AdminResponseDTO?> LoginAsync(AdminLoginRequestDTO request);
        Task<int> CreateAdminAsync(AdminDTO dto);
        Task<bool> UpdateAdminAsync(int id, AdminDTO dto);
        Task<IEnumerable<AdminResponseDTO>> GetAllAdminsAsync();
    }
}
