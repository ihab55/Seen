using Microsoft.Data.SqlClient;
using SeenCL.Domain.Entities;
using SeenCL.Repositories;
using SeenDAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;

namespace SeenDAL.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IDatabaseHelper _dbHelper;

        public AdminRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        private SqlConnection CreateSqlConnection() => _dbHelper.CreateConnection();

        public Admin? GetById(int id)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Admins_GetByID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AdminID", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read()) return Map(reader);
            }
            catch { }
            return null;
        }

        public IEnumerable<Admin> GetAll()
        {
            List<Admin> list = new();
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Admins_GetAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read()) list.Add(Map(reader));
            }
            catch { }
            return list;
        }

        public int Create(Admin entity)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Admins_Create", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AdminName", entity.AdminName);
                cmd.Parameters.AddWithValue("@Email", entity.Email);
                cmd.Parameters.AddWithValue("@PasswordHash", entity.PasswordHash);
                cmd.Parameters.AddWithValue("@IsActive", entity.IsActive);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch { return -1; }
        }

        public bool Update(Admin entity)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Admins_Update", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AdminID", entity.AdminID);
                cmd.Parameters.AddWithValue("@AdminName", entity.AdminName);
                cmd.Parameters.AddWithValue("@Email", entity.Email);
                cmd.Parameters.AddWithValue("@IsActive", entity.IsActive);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }

        public bool Delete(int id)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Admins_Delete", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AdminID", id);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }

        public Admin? GetByEmail(string email)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Admins_GetByEmail", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read()) return Map(reader);
            }
            catch { }
            return null;
        }

        private Admin Map(SqlDataReader reader)
        {
            return new Admin
            {
                AdminID = reader.GetInt32("AdminID"),
                AdminName = reader.GetString("AdminName"),
                Email = reader.GetString("Email"),
                PasswordHash = reader.GetString("PasswordHash"),
                CreatedAt = reader.GetDateTime("CreatedAt"),
                IsActive = reader.GetBoolean("IsActive")
            };
        }
    }
}