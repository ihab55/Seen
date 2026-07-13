using Microsoft.Data.SqlClient;
using SeenCL.Domain.Entities;
using SeenCL.Repositories;
using SeenDAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;

namespace SeenDAL.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly IDatabaseHelper _dbHelper;

        public DeviceRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        private SqlConnection CreateSqlConnection() => _dbHelper.CreateConnection();

        public Device? GetById(int id)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Devices_GetByID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DeviceID", id);

                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read()) return Map(reader);
            }
            catch { }
            return null;
        }

        public IEnumerable<Device> GetAll()
        {
            List<Device> list = new();
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Devices_GetAll", conn); // Assuming SP exists
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(Map(reader));
                }
            }
            catch { }
            return list;
        }

        public int Create(Device entity)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Devices_RegisterOrUpdate", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DeviceName", entity.DeviceName);
                cmd.Parameters.AddWithValue("@DeviceType", entity.DeviceType);
                cmd.Parameters.AddWithValue("@SerialNumber", entity.SerialNumber);
                cmd.Parameters.AddWithValue("@MacAddress", entity.MacAddress);
                cmd.Parameters.AddWithValue("@IsActive", entity.IsActive);

                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
            catch { return -1; }
        }

        public bool Update(Device entity)
        {
            // Re-using Create logic for RegisterOrUpdate if that's how the SP works
            return Create(entity) > 0;
        }

        public bool Delete(int id)
        {
            // Placeholder for delete logic
            return false;
        }

        public Device? GetByUniqueFields(string identifier)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Devices_GetByUniqueFields", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Identifier", identifier);

                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read()) return Map(reader);
            }
            catch { }
            return null;
        }

        public bool SetStatus(int deviceId, bool isActive)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Devices_SetStatus", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DeviceID", deviceId);
                cmd.Parameters.AddWithValue("@IsActive", isActive);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }

        private Device Map(SqlDataReader reader)
        {
            return new Device
            {
                DeviceID = reader.GetInt32("DeviceID"),
                DeviceName = reader.GetString("DeviceName"),
                DeviceType = reader.GetString("DeviceType"),
                SerialNumber = reader.GetString("SerialNumber"),
                MacAddress = reader.GetString("MacAddress"),
                IsActive = reader.GetBoolean("IsActive"),
                RegisteredAt = reader.IsDBNull("RegisteredAt") ? null : reader.GetDateTime("RegisteredAt")
            };
        }
    }
}