using Microsoft.Data.SqlClient;
using SeenCL.Domain.Entities;
using SeenCL.Repositories;
using SeenDAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;

namespace SeenDAL.Repositories
{
    public class SensorRepository : ISensorRepository
    {
        private readonly IDatabaseHelper _dbHelper;

        public SensorRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        private SqlConnection CreateSqlConnection() => _dbHelper.CreateConnection();

        public Sensor? GetById(int id)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Sensors_GetByID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SensorID", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read()) return Map(reader);
            }
            catch { }
            return null;
        }

        public IEnumerable<Sensor> GetAll()
        {
            List<Sensor> list = new();
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Sensors_GetAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read()) list.Add(Map(reader));
            }
            catch { }
            return list;
        }

        public int Create(Sensor entity)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Sensors_Create", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SensorName", entity.SensorName);
                cmd.Parameters.AddWithValue("@SensorType", entity.SensorType);
                cmd.Parameters.AddWithValue("@Unit", entity.Unit);
                cmd.Parameters.AddWithValue("@MinSafeValue", entity.MinSafeValue);
                cmd.Parameters.AddWithValue("@MaxSafeValue", entity.MaxSafeValue);
                cmd.Parameters.AddWithValue("@Description", (object?)entity.Description ?? DBNull.Value);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch { return -1; }
        }

        public bool Update(Sensor entity)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Sensors_Update", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SensorID", entity.SensorID);
                cmd.Parameters.AddWithValue("@SensorName", entity.SensorName);
                cmd.Parameters.AddWithValue("@SensorType", entity.SensorType);
                cmd.Parameters.AddWithValue("@Unit", entity.Unit);
                cmd.Parameters.AddWithValue("@MinSafeValue", entity.MinSafeValue);
                cmd.Parameters.AddWithValue("@MaxSafeValue", entity.MaxSafeValue);
                cmd.Parameters.AddWithValue("@Description", (object?)entity.Description ?? DBNull.Value);
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
                using var cmd = new SqlCommand("SP_Sensors_Delete", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SensorID", id);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }

        private Sensor Map(SqlDataReader reader)
        {
            return new Sensor
            {
                SensorID = reader.GetInt32("SensorID"),
                SensorName = reader.GetString("SensorName"),
                SensorType = reader.GetString("SensorType"),
                Unit = reader.GetString("Unit"),
                MinSafeValue = reader.GetDouble("MinSafeValue"),
                MaxSafeValue = reader.GetDouble("MaxSafeValue"),
                Description = reader.IsDBNull("Description") ? null : reader.GetString("Description")
            };
        }

    }
}