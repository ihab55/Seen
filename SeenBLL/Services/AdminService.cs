using BCrypt.Net;
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

        /// <remarks>
        /// NOTE: Token issuance is handled by IAuthService.AdminLoginAsync.
        /// This method only validates credentials and returns admin info.
        /// </remarks>
        public async Task<AdminResponseDTO?> LoginAsync(AdminLoginRequestDTO request)
        {
            var admin = await Task.FromResult(_repository.GetByEmail(request.Email));
            if (admin == null) return null;

            // BCrypt verification
            if (!BCrypt.Net.BCrypt.Verify(request.Password, admin.PasswordHash)) return null;

            return MapToResponseDTO(admin);
        }

        public async Task<int> CreateAdminAsync(AdminDTO dto)
        {
            var admin = new Admin
            {
                AdminName    = dto.AdminName,
                Email        = dto.Email,
                // Hash the initial password with BCrypt before storing
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password ?? "ChangeMe123!"),
                CreatedAt    = DateTime.UtcNow,
                IsActive     = true
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
