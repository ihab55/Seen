using Microsoft.Data.SqlClient;
using SeenCL.Domain.Entities;
using SeenCL.DTOs;
using SeenCL.Repositories;
using SeenDAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;

namespace SeenDAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDatabaseHelper _dbHelper;

        public UserRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        private SqlConnection CreateSqlConnection() => _dbHelper.CreateConnection();

        public User? GetById(int id)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Users_GetByID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", id);

                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (!reader.Read()) return null;

                return MapUser(reader);
            }
            catch
            {
                return null;
            }
        }

        public UserResponseDTO? GetByUsername(string Username)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Users_GetByUsername", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", Username);

                conn.Open();
                using var reader = cmd.ExecuteReader();
                UserResponseDTO? responseDTO = new UserResponseDTO();
                if (reader.Read())
                {
                    responseDTO.UserID = reader.GetInt32(reader.GetOrdinal("UserID"));
                    responseDTO.FullName = reader.IsDBNull(reader.GetOrdinal("FullName")) ? string.Empty : reader.GetString(reader.GetOrdinal("FullName"));
                    responseDTO.Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? string.Empty : reader.GetString(reader.GetOrdinal("Email"));
                    responseDTO.IsProfileCompleted = !reader.IsDBNull(reader.GetOrdinal("IsProfileCompleted")) && reader.GetBoolean(reader.GetOrdinal("IsProfileCompleted"));
                    responseDTO.Height = reader.IsDBNull(reader.GetOrdinal("Height")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("Height"));
                    responseDTO.Weight = reader.IsDBNull(reader.GetOrdinal("Weight")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("Weight"));
                    responseDTO.FateRate = reader.IsDBNull(reader.GetOrdinal("FateRate")) ? null : (double?)Convert.ToDouble(reader["FateRate"]);
                    responseDTO.IsCoach = !reader.IsDBNull(reader.GetOrdinal("IsCoach")) && reader.GetBoolean(reader.GetOrdinal("IsCoach"));
                    responseDTO.ImagePath = reader.IsDBNull(reader.GetOrdinal("ImagePath")) ? null : reader.GetString(reader.GetOrdinal("ImagePath"));
                    
                    return responseDTO;
                }

            }
            catch
            {
                return null;
            }
            return null;
        }
        //====================================
        // Admin-specific method to get all users with more details
        //====================================
        public IEnumerable<UserAdminResponseDTO>? GetAll()
        {
            List<UserAdminResponseDTO> list = new();
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Users_GetAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new UserAdminResponseDTO
                    {
                        UserID = reader.GetFieldValue<int>(reader.GetOrdinal("UserID")),
                        FullName = reader.IsDBNull(reader.GetOrdinal("FullName")) ? string.Empty : reader.GetString(reader.GetOrdinal("FullName")),
                        Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? string.Empty : reader.GetString(reader.GetOrdinal("Email")),
                        UserName = reader.IsDBNull(reader.GetOrdinal("UserName")) ? string.Empty : reader.GetString(reader.GetOrdinal("UserName")),
                        IsProfileCompleted = !reader.IsDBNull(reader.GetOrdinal("IsProfileCompleted")) && reader.GetBoolean(reader.GetOrdinal("IsProfileCompleted")),
                        IsCoach = !reader.IsDBNull(reader.GetOrdinal("IsCoach")) && reader.GetBoolean(reader.GetOrdinal("IsCoach")),
                        IsDeleted = !reader.IsDBNull(reader.GetOrdinal("IsDeleted")) && reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
                    });
                }
            }
            catch
            {
                return null;
            }
            return list;
        }

        public int Create(User entity)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Users_Create", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FirstName", entity.FirstName);
                cmd.Parameters.AddWithValue("@LastName", entity.LastName);
                cmd.Parameters.AddWithValue("@UserName", entity.UserName);
                cmd.Parameters.AddWithValue("@Email", entity.Email);
                cmd.Parameters.AddWithValue("@PasswordHash", entity.PasswordHash);
                cmd.Parameters.AddWithValue("@ImagePath", entity.ImagePath ?? (object)DBNull.Value);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch
            {
                return -1;
            }
        }

        public bool Update(User entity)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Users_UpdateByAdmin", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", entity.UserID);
                cmd.Parameters.AddWithValue("@FirstName", entity.FirstName);
                cmd.Parameters.AddWithValue("@LastName", entity.LastName);
                cmd.Parameters.AddWithValue("@UserName", entity.UserName);
                cmd.Parameters.AddWithValue("@Email", entity.Email);
                cmd.Parameters.AddWithValue("@Height", entity.Height ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Weight", entity.Weight ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@FateRate", entity.FateRate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DeviceID", entity.DeviceID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@IsCoach", entity.IsCoach);
                cmd.Parameters.AddWithValue("@IsDeleted", entity.IsDeleted);
                cmd.Parameters.AddWithValue("@ImagePath", entity.ImagePath ?? (object)DBNull.Value);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            // Soft delete usually
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Users_SoftDelete", conn); // Assuming this SP exists
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", id);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }

        public User? GetByEmailOrUsername(string login)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Users_GetForLogin", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserNameOrEmail", login);

                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (!reader.Read()) return null;

                return MapUser(reader);
            }
            catch
            {
                return null;
            }
        }

        public bool AssignDevice(int userId, int deviceId)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Users_AssignDevice", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@DeviceID", deviceId);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveDevice(int userId)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Users_DeleteDevice", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", userId);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool HardDelete(int userId)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Users_HardDeleteByAdmin", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", userId);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
            catch
            {
                return false;
            }
        }

        private User MapUser(SqlDataReader reader)
        {
            return new User
            {
                UserID = reader.GetFieldValue<int>(reader.GetOrdinal("UserID")),
                FirstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? string.Empty : reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? string.Empty : reader.GetString(reader.GetOrdinal("LastName")),
                UserName = reader.IsDBNull(reader.GetOrdinal("UserName")) ? string.Empty : reader.GetString(reader.GetOrdinal("UserName")),
                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? string.Empty : reader.GetString(reader.GetOrdinal("Email")),
                PasswordHash = reader.IsDBNull(reader.GetOrdinal("PasswordHash")) ? string.Empty : reader.GetString(reader.GetOrdinal("PasswordHash")),
                Height = reader.IsDBNull(reader.GetOrdinal("Height")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("Height")),
                Weight = reader.IsDBNull(reader.GetOrdinal("Weight")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("Weight")),
                FateRate = reader.IsDBNull(reader.GetOrdinal("FateRate")) ? null : (double?)Convert.ToDouble(reader["FateRate"]),
                DeviceID = reader.IsDBNull(reader.GetOrdinal("DeviceID")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("DeviceID")),
                IsCoach = !reader.IsDBNull(reader.GetOrdinal("IsCoach")) && reader.GetBoolean(reader.GetOrdinal("IsCoach")),
                IsProfileCompleted = !reader.IsDBNull(reader.GetOrdinal("IsProfileCompleted")) && reader.GetBoolean(reader.GetOrdinal("IsProfileCompleted")),
                IsDeleted = !reader.IsDBNull(reader.GetOrdinal("IsDeleted")) && reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                ImagePath = reader.IsDBNull(reader.GetOrdinal("ImagePath")) ? null : reader.GetString(reader.GetOrdinal("ImagePath")),
                CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
            };
        }

        public IEnumerable<UserSummaryDTO> GetAllUserSummaries()
        {
            throw new NotImplementedException();
        }
    }
}

