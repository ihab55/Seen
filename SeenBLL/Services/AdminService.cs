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
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _repository;

        public AdminService(IAdminRepository repository)
        {
            _repository = repository;
        }

        public async Task<AdminResponseDTO?> LoginAsync(AdminLoginRequestDTO request)
        {
            var admin = await Task.FromResult(_repository.GetByEmail(request.Email));
            if (admin == null || admin.PasswordHash != request.Password) return null; // Simple check for now
            return MapToResponseDTO(admin);
        }

        public async Task<int> CreateAdminAsync(AdminDTO dto)
        {
            var admin = new Admin
            {
                AdminName = dto.AdminName,
                Email = dto.Email,
                PasswordHash = "InitialPassword", // Should be hashed
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
            return await Task.FromResult(_repository.Create(admin));
        }

        public async Task<bool> UpdateAdminAsync(int id, AdminDTO dto)
        {
            var admin = await Task.FromResult(_repository.GetById(id));
            if (admin == null) return false;
            admin.AdminName = dto.AdminName;
            admin.Email = dto.Email;
            return await Task.FromResult(_repository.Update(admin));
        }

        public async Task<IEnumerable<AdminResponseDTO>> GetAllAdminsAsync()
        {
            var admins = await Task.FromResult(_repository.GetAll());
            return admins.Select(MapToResponseDTO);
        }

        private AdminResponseDTO MapToResponseDTO(Admin a)
        {
            return new AdminResponseDTO(
                a.AdminID,
                a.AdminName,
                a.Email,
                a.CreatedAt,
                a.IsActive
            );
        }
    }
}
