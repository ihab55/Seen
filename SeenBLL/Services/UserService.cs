using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using SeenCL.Domain.Entities;
using SeenCL.DTOs;
using SeenCL.Repositories;
using SeenCL.Services;
using SeenCL.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeenBLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        #region IUserService Implementation

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await Task.FromResult(_userRepository.GetById(id));
        }

        public async Task<UserResponseDTO?> GetUserResponseByIdAsync(int id)
        {
            var user = await Task.FromResult(_userRepository.GetById(id));
            if (user == null) return null;

            var dto = MapToResponseDTO(user);

            // Use FileHelper to load image data
            string imageFolder = _configuration["FileSettings:SeenImagePath"] ?? string.Empty;
            if (FileHelper.CreateFolderIfNotExists(imageFolder))
            {
            dto.ImageData = FileHelper.GetImageData(imageFolder, user.ImagePath);
            }
            return dto;
        }
        public async Task<UserResponseDTO?> GetUserResponseByUsernameAsync(string Username)
        {
            var user = await Task.FromResult(_userRepository.GetByUsername(Username));
            
            // Use FileHelper to load image data
            string imageFolder = _configuration["FileSettings:SeenImagePath"] ?? string.Empty;
            if (FileHelper.CreateFolderIfNotExists(imageFolder))
            {
                user.ImageData = FileHelper.GetImageData(imageFolder, user.ImagePath);
            }
            return user;
        }

        /// <remarks>
        /// NOTE: Authentication (JWT + token issuance) has been moved to AuthService.LoginAsync.
        /// This method remains for internal use (e.g., profile fetch after auth) but does NOT
        /// issue tokens. Prefer IAuthService.LoginAsync for actual login flows.
        /// </remarks>
        public async Task<UserResponseDTO?> LoginAsync(UserLoginDTO loginDto)
        {
            var user = await Task.FromResult(_userRepository.GetByEmailOrUsername(loginDto.UserName));
            if (user == null) return null;

            // Use BCrypt.Verify to safely compare the plain-text password against the stored hash
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                return null;

            return MapToResponseDTO(user);
        }

        public async Task<bool> AssignDeviceAsync(int userId, int deviceId)
        {
            return await Task.FromResult(_userRepository.AssignDevice(userId, deviceId));
        }

        public async Task<bool> RemoveDeviceAsync(int userId)
        {
            return await Task.FromResult(_userRepository.RemoveDevice(userId));
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await Task.FromResult(_userRepository.Delete(id));
        }

        public async Task<bool> HardDeleteUserAsync(int id)
        {
            return await Task.FromResult(_userRepository.HardDelete(id));
        }

        public async Task<IEnumerable<UserAdminResponseDTO>?> GetAllUserResponsesAsync()
        {
            return await Task.FromResult(_userRepository.GetAll());
        }

        public async Task<IEnumerable<UserSummaryDTO>> GetAllUserSummariesAsync()
        {
            return await Task.FromResult(_userRepository.GetAllUserSummaries());
        }

        public async Task<int> CreateUserAsync(UserCreateDTO dto)
        {
            // Hash the password with BCrypt before storing — NEVER store plain-text passwords
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                FirstName          = dto.FirstName,
                LastName           = dto.LastName,
                UserName           = dto.UserName,
                Email              = dto.Email,
                PasswordHash       = passwordHash,
                CreatedAt          = DateTime.UtcNow,
                IsProfileCompleted = false,
                IsDeleted          = false
            };
            return await Task.FromResult(_userRepository.Create(user));
        }

        public async Task<bool> UpdateUserProfileAsync(int userId, UserProfileUpdateDTO dto)
        {
            var user = await Task.FromResult(_userRepository.GetById(userId));
            if (user == null) return false;

            // Handle Image Upload if a file is provided
            if (dto.ImageFile != null)
            {
                string imageFolder = _configuration["FileSettings:SeenImagePath"] ?? string.Empty;

                // Delete old image if it exists
                if (!string.IsNullOrEmpty(user.ImagePath))
                {
                    FileHelper.DeleteFile(imageFolder, user.ImagePath);
                }

                // Save new image
                string? newFileName = await FileHelper.SaveFileAsync(dto.ImageFile, imageFolder);
                if (newFileName != null)
                {
                    user.ImagePath = newFileName;
                }
            }

            user.Height = dto.Height ?? user.Height;
            user.Weight = dto.Weight ?? user.Weight;
            user.FateRate = dto.FateRate ?? user.FateRate;
            user.IsProfileCompleted = true;
            user.UpdatedAt = DateTime.UtcNow;

            return await Task.FromResult(_userRepository.Update(user));
        }

        public async Task<bool> UpdateUserByAdminAsync(UserAdminUpdateDTO dto)
        {
            var user = await Task.FromResult(_userRepository.GetById(dto.UserID));
            if (user == null) return false;

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.UserName = dto.UserName;
            user.Email = dto.Email;
            user.Height = dto.Height;
            user.Weight = dto.Weight;
            user.FateRate = dto.FateRate;
            user.DeviceID = dto.DeviceID;
            user.IsDeleted = dto.IsDeleted;
            user.IsProfileCompleted = dto.IsProfileCompleted;
            user.UpdatedAt = DateTime.UtcNow;

            return await Task.FromResult(_userRepository.Update(user));
        }

        #endregion

        #region Mapping Helpers

        private UserResponseDTO MapToResponseDTO(User user)
        {
            return new UserResponseDTO
            {
                UserID = user.UserID,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                IsProfileCompleted = user.IsProfileCompleted,
                Height = user.Height,
                Weight = user.Weight,
                FateRate = user.FateRate,
                IsCoach = user.IsCoach,
                ImagePath = user.ImagePath
            };
        }

        private UserSummaryDTO MapToSummaryDTO(User user)
        {
            return new UserSummaryDTO
            {
                UserId = user.UserID,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                UserName = user.UserName,
                IsProfileCompleted = user.IsProfileCompleted,
                IsCoach = user.IsCoach,
                IsDeleted = user.IsDeleted
            };
        }

        #endregion
    }
}
