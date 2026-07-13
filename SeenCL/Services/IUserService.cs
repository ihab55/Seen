using SeenCL.Domain.Entities;
using SeenCL.DTOs;
using System.Collections.Generic;

namespace SeenCL.Services
{
    public interface IUserService
    {
        Task<bool> AssignDeviceAsync(int userId, int deviceId);
        Task<bool> RemoveDeviceAsync(int userId);
        Task<User?> GetUserByIdAsync(int id);
        Task<UserResponseDTO?> GetUserResponseByIdAsync(int id);
        Task<UserResponseDTO?> LoginAsync(UserLoginDTO loginDto);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> HardDeleteUserAsync(int id);
        Task<IEnumerable<UserAdminResponseDTO>?> GetAllUserResponsesAsync();
        Task<IEnumerable<UserSummaryDTO>> GetAllUserSummariesAsync();
        Task<int> CreateUserAsync(UserCreateDTO dto);
        Task<bool> UpdateUserProfileAsync(int userId, UserProfileUpdateDTO dto);
        Task<bool> UpdateUserByAdminAsync(UserAdminUpdateDTO dto);
        Task<UserResponseDTO?> GetUserResponseByUsernameAsync(string Username);
    }
}
