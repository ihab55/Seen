using Microsoft.Data.SqlClient;
using SeenCL.Domain.Entities;
using SeenCL.Repositories;
using SeenDAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;

namespace SeenDAL.Repositories
{
    public class ProgramCommentRepository : IProgramCommentRepository
    {
        private readonly IDatabaseHelper _dbHelper;

        public ProgramCommentRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        private SqlConnection CreateSqlConnection() => _dbHelper.CreateConnection();

        public ProgramComment? GetById(int id)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_ProgramComments_GetByID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CommentID", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read()) return Map(reader);
            }
            catch { }
            return null;
        }

        public IReadOnlyList<ProgramComment> GetAll()
        {
            List<ProgramComment> list = new();
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_ProgramComments_GetAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read()) list.Add(Map(reader));
            }
            catch { }
            return list;
        }

        public int Create(ProgramComment entity)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_ProgramComments_Create", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProgramID", entity.ProgramID);
                cmd.Parameters.AddWithValue("@MemberID", entity.MemberID);
                cmd.Parameters.AddWithValue("@CommentText", entity.CommentText);
                cmd.Parameters.AddWithValue("@ParentCommentID", (object?)entity.ParentCommentID ?? DBNull.Value);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch { return -1; }
        }

        public bool Update(ProgramComment entity)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_ProgramComments_Update", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CommentID", entity.CommentID);
                cmd.Parameters.AddWithValue("@CommentText", entity.CommentText);
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
                using var cmd = new SqlCommand("SP_ProgramComments_Delete", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CommentID", id);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }

        public IEnumerable<ProgramComment> GetByProgramId(int programId)
        {
            List<ProgramComment> list = new();
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_ProgramComments_GetByProgramID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProgramID", programId);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read()) list.Add(Map(reader));
            }
            catch { }
            return list;
        }

        private ProgramComment Map(SqlDataReader reader)
        {
            return new ProgramComment
            {
                CommentID = reader.GetInt32("CommentID"),
                ProgramID = reader.GetInt32("ProgramID"),
                MemberID = reader.GetInt32("MemberID"),
                CommentText = reader.GetString("CommentText"),
                ParentCommentID = reader.IsDBNull("ParentCommentID") ? null : reader.GetInt32("ParentCommentID"),
                IsDeleted = reader.GetBoolean("IsDeleted"),
                CreatedAt = reader.GetDateTime("CreatedAt")
            };
        }
    }
}