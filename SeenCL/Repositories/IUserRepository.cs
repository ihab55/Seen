using SeenCL.Domain.Entities;
using SeenCL.DTOs;
using SeenCL.Interfaces;
using System.Collections.Generic;

namespace SeenCL.Repositories
{
    public interface IUserRepository : IRepository<User, int>
    {
        User? GetByEmailOrUsername(string login);
        UserResponseDTO? GetByUsername(string Username);
        IEnumerable<UserSummaryDTO> GetAllUserSummaries();
        IEnumerable<UserAdminResponseDTO>? GetAll();
        bool AssignDevice(int userId, int deviceId);
        bool RemoveDevice(int userId);
        bool HardDelete(int userId);
    }
}
