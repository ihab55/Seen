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
    public class CoachApprovalRepository : ICoachApprovalRepository
    {
        private readonly IDatabaseHelper _dbHelper;

        public CoachApprovalRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        private SqlConnection CreateSqlConnection() => _dbHelper.CreateConnection();

        public CoachApproval? GetById(int id)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_CoachApprovals_GetByID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ApprovalID", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read()) return Map(reader);
            }
            catch { }
            return null;
        }

        public IEnumerable<CoachApproval> GetAll()
        {
            List<CoachApproval> list = new();
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_CoachApprovals_GetAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read()) list.Add(Map(reader));
            }
            catch { }
            return list;
        }

        public int Create(CoachApproval entity)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_CoachApprovals_Create", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", entity.UserID);
                cmd.Parameters.AddWithValue("@Bio", entity.Bio);
                cmd.Parameters.AddWithValue("@CVUrl", entity.CVUrl);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch { return -1; }
        }

        public bool Update(CoachApproval entity)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_CoachApprovals_Update", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ApprovalID", entity.ApprovalID);
                cmd.Parameters.AddWithValue("@UserID", entity.UserID);
                cmd.Parameters.AddWithValue("@AdminID", (object?)entity.ApprovedByAdminID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", (byte)entity.Status);
                cmd.Parameters.AddWithValue("@Bio", entity.Bio);
                cmd.Parameters.AddWithValue("@CVUrl", entity.CVUrl);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
            catch { return false; }
        }

        public bool Delete(int id)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_CoachApprovals_Delete", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ApprovalID", id);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }

        public CoachApproval? GetByUserId(int userId)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_CoachApprovals_GetByUserID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", userId);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read()) return Map(reader);
            }
            catch { }
            return null;
        }

        public bool RevokeCoachApproval(int approvalId, int adminId)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_CoachApprovals_Revoke", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ApprovalID", approvalId);
                cmd.Parameters.AddWithValue("@AdminID", adminId);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
            catch { return false; }
        }

        private CoachApproval Map(SqlDataReader reader)
        {
            return new CoachApproval
            {
                ApprovalID = reader.GetInt32("ApprovalID"),
                UserID = reader.GetInt32("UserID"),
                ApprovedByAdminID = reader.IsDBNull("ApprovedByAdminID") ? (int?)null : reader.GetInt32("ApprovedByAdminID"),
                Status = (ApprovalStatus)reader.GetByte("Status"),
                RequestedAt = reader.GetDateTime("RequestedAt"),
                ApprovedAt = reader.IsDBNull("ApprovedAt") ? (DateTime?)null : reader.GetDateTime("ApprovedAt"),
                Bio = reader.GetString("Bio"),
                CVUrl = reader.GetString("CVUrl")
            };
        }
    }
}