using Microsoft.Data.SqlClient;
using SeenCL.Domain.Entities;
using SeenCL.Repositories;
using SeenDAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;

namespace SeenDAL.Repositories
{
    public class SensorDataRepository : ISensorDataRepository
    {
        private readonly IDatabaseHelper _dbHelper;

        public SensorDataRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        private SqlConnection CreateSqlConnection() => _dbHelper.CreateConnection();

        public SensorData? GetById(long id)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_SensorData_GetByID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DataID", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read()) return Map(reader);
            }
            catch { }
            return null;
        }

        public IReadOnlyList<SensorData> GetAll()
        {
            List<SensorData> list = new();
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_SensorData_GetAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read()) list.Add(Map(reader));
            }
            catch { }
            return list;
        }

        public long Create(SensorData entity)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_SensorData_Add", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Reader", entity.Reader);
                cmd.Parameters.AddWithValue("@SensorID", entity.SensorID);
                cmd.Parameters.AddWithValue("@DeviceID", entity.DeviceID);
                conn.Open();
                return Convert.ToInt64(cmd.ExecuteScalar());
            }
            catch { return -1; }
        }

        public bool Update(SensorData entity) => false;

        public bool Delete(long id)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_SensorData_Delete", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DataID", id);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }

        public IEnumerable<SensorData> GetBySensorId(int sensorId)
        {
            List<SensorData> list = new();
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_SensorData_GetBySensorID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SensorID", sensorId);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read()) list.Add(Map(reader));
            }
            catch { }
            return list;
        }

        private SensorData Map(SqlDataReader reader)
        {
            return new SensorData
            {
                DataID = reader.GetInt64("DataID"),
                Reader = reader.GetDouble("Reader"),
                Timestamp = reader.GetDateTime("Timestamp"),
                SensorID = reader.GetInt32("SensorID"),
                DeviceID = reader.GetInt32("DeviceID")
            };
        }
    }
}