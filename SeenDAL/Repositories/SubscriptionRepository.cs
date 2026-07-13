using Microsoft.Data.SqlClient;
using SeenCL.Domain.Entities;
using SeenCL.Repositories;
using SeenDAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;

namespace SeenDAL.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly IDatabaseHelper _dbHelper;

        public SubscriptionRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        private SqlConnection CreateSqlConnection() => _dbHelper.CreateConnection();

        public Subscription? GetById(int id)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Subscriptions_GetByID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SubscriptionID", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read()) return Map(reader);
            }
            catch { }
            return null;
        }

        public IEnumerable<Subscription> GetAll()
        {
            List<Subscription> list = new();
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Subscriptions_GetAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read()) list.Add(Map(reader));
            }
            catch { }
            return list;
        }

        public int Create(Subscription entity)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Subscriptions_Create", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlanName", entity.PlanName);
                cmd.Parameters.AddWithValue("@Description", (object?)entity.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MaxPlayers", entity.MaxPlayers);
                cmd.Parameters.AddWithValue("@DurationDays", entity.DurationDays);
                cmd.Parameters.AddWithValue("@Price", entity.Price);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch { return -1; }
        }

        public bool Update(Subscription entity)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Subscriptions_Update", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SubscriptionID", entity.SubscriptionID);
                cmd.Parameters.AddWithValue("@PlanName", entity.PlanName);
                cmd.Parameters.AddWithValue("@Description", (object?)entity.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MaxPlayers", entity.MaxPlayers);
                cmd.Parameters.AddWithValue("@DurationDays", entity.DurationDays);
                cmd.Parameters.AddWithValue("@Price", entity.Price);
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
                using var cmd = new SqlCommand("SP_Subscriptions_Delete", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SubscriptionID", id);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }

        private Subscription Map(SqlDataReader reader)
        {
            return new Subscription
            {
                SubscriptionID = reader.GetInt32("SubscriptionID"),
                PlanName = reader.GetString("PlanName"),
                Description = reader.IsDBNull("Description") ? null : reader.GetString("Description"),
                MaxPlayers = reader.GetInt32("MaxPlayers"),
                DurationDays = reader.GetInt32("DurationDays"),
                Price = reader.GetDecimal("Price"),
                CreatedAt = reader.GetDateTime("CreatedAt")
            };
        }
    }
}