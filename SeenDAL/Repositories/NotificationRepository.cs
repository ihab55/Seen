using Microsoft.Data.SqlClient;
using SeenCL.Domain.Entities;
using SeenCL.Domain.Enums;
using SeenCL.Repositories;
using SeenDAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;

namespace SeenDAL.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IDatabaseHelper _dbHelper;

        public NotificationRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        private SqlConnection CreateSqlConnection() => _dbHelper.CreateConnection();

        public Notification? GetById(int id)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Notifications_GetByID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NotificationID", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read()) return Map(reader);
            }
            catch { }
            return null;
        }

        public IReadOnlyList<Notification> GetAll()
        {
            List<Notification> list = new();
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Notifications_GetAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read()) list.Add(Map(reader));
            }
            catch { }
            return list;
        }

        public int Create(Notification entity)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Notifications_Create", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", entity.UserID);
                cmd.Parameters.AddWithValue("@Title", entity.Title);
                cmd.Parameters.AddWithValue("@Body", entity.Body);
                cmd.Parameters.AddWithValue("@NotificationType", (byte)entity.NotificationType);
                cmd.Parameters.AddWithValue("@TargetID", (object?)entity.TargetID ?? DBNull.Value);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch { return -1; }
        }

        public bool Update(Notification entity) => false;

        public bool Delete(int id)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Notifications_Delete", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NotificationID", id);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }

        public IEnumerable<Notification> GetByUserId(int userId)
        {
            List<Notification> list = new();
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Notifications_GetByUserID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", userId);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read()) list.Add(Map(reader));
            }
            catch { }
            return list;
        }

        public bool MarkAsRead(int notificationId)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Notifications_MarkAsRead", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NotificationID", notificationId);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }

        private Notification Map(SqlDataReader reader)
        {
            return new Notification
            {
                NotificationID = reader.GetInt32("NotificationID"),
                UserID = reader.GetInt32("UserID"),
                Title = reader.GetString("Title"),
                Body = reader.GetString("Body"),
                NotificationType = (NotificationType)reader.GetByte("NotificationType"),
                IsRead = reader.GetBoolean("IsRead"),
                CreatedAt = reader.GetDateTime("CreatedAt"),
                TargetID = reader.IsDBNull("TargetID") ? null : reader.GetInt32("TargetID")
            };
        }
    }
}