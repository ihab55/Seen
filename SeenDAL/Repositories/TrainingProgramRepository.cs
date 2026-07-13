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
    public class TrainingProgramRepository : ITrainingProgramRepository
    {
        private readonly IDatabaseHelper _dbHelper;

        public TrainingProgramRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        private SqlConnection CreateSqlConnection() => _dbHelper.CreateConnection();

        public TrainingProgram? GetById(int id)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_TrainingPrograms_GetByID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProgramID", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read()) return Map(reader);
            }
            catch { }
            return null;
        }

        public IReadOnlyList<TrainingProgram> GetAll()
        {
            List<TrainingProgram> list = new();
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_TrainingPrograms_GetAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read()) list.Add(Map(reader));
            }
            catch { }
            return list;
        }

        public int Create(TrainingProgram entity)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_TrainingPrograms_Create", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TeamID", entity.TeamID);
                cmd.Parameters.AddWithValue("@TeamMemberID", entity.TeamMemberID);
                cmd.Parameters.AddWithValue("@ProgramName", entity.ProgramName);
                cmd.Parameters.AddWithValue("@Goal", entity.Goal);
                cmd.Parameters.AddWithValue("@IntensityLevel", entity.IntensityLevel);
                cmd.Parameters.AddWithValue("@StartDate", entity.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", entity.EndDate);
                cmd.Parameters.AddWithValue("@Status", (byte)entity.Status);
                cmd.Parameters.AddWithValue("@Notes", (object?)entity.Notes ?? DBNull.Value);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch { return -1; }
        }

        public bool Update(TrainingProgram entity)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_TrainingPrograms_Update", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProgramID", entity.ProgramID);
                cmd.Parameters.AddWithValue("@ProgramName", entity.ProgramName);
                cmd.Parameters.AddWithValue("@Goal", entity.Goal);
                cmd.Parameters.AddWithValue("@IntensityLevel", entity.IntensityLevel);
                cmd.Parameters.AddWithValue("@EndDate", entity.EndDate);
                cmd.Parameters.AddWithValue("@Status", (byte)entity.Status);
                cmd.Parameters.AddWithValue("@Notes", (object?)entity.Notes ?? DBNull.Value);
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
                using var cmd = new SqlCommand("SP_TrainingPrograms_Delete", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProgramID", id);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }

        public IEnumerable<TrainingProgram> GetByTeamId(int teamId)
        {
            List<TrainingProgram> list = new();
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_TrainingPrograms_GetByTeamID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TeamID", teamId);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read()) list.Add(Map(reader));
            }
            catch { }
            return list;
        }

        private TrainingProgram Map(SqlDataReader reader)
        {
            return new TrainingProgram
            {
                ProgramID = reader.GetInt32("ProgramID"),
                TeamID = reader.GetInt32("TeamID"),
                TeamMemberID = reader.GetInt32("TeamMemberID"),
                ProgramName = reader.GetString("ProgramName"),
                Goal = reader.GetString("Goal"),
                IntensityLevel = reader.GetByte("IntensityLevel"),
                StartDate = reader.GetDateTime("StartDate"),
                EndDate = reader.GetDateTime("EndDate"),
                Status = (ProgramStatus)reader.GetByte("Status"),
                Notes = reader.IsDBNull("Notes") ? null : reader.GetString("Notes"),
                CreatedAt = reader.GetDateTime("CreatedAt")
            };
        }
    }
}