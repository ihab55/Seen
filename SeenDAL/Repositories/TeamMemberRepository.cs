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
    public class TeamMemberRepository : ITeamMemberRepository
    {
        private readonly IDatabaseHelper _dbHelper;

        public TeamMemberRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        private SqlConnection CreateSqlConnection() => _dbHelper.CreateConnection();

        public TeamMember? GetById(int id)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_TeamMembers_GetByID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MemberID", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read()) return Map(reader);
            }
            catch { }
            return null;
        }

        public IEnumerable<TeamMember> GetAll()
        {
            List<TeamMember> list = new();
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_TeamMembers_GetAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read()) list.Add(Map(reader));
            }
            catch { }
            return list;
        }

        public int Create(AddTeamMemberDTO entity)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_TeamMembers_Add", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TeamID", entity.TeamID);
                cmd.Parameters.AddWithValue("@PlayerID", entity.PlayerID);
                cmd.Parameters.AddWithValue("@Position", entity.Position);
                cmd.Parameters.AddWithValue("@JerseyNumber", entity.JerseyNumber);
                cmd.Parameters.AddWithValue("@IsInjured", entity.IsInjured);
                cmd.Parameters.AddWithValue("@IsRequestByCoach", entity.IsRequestByCoach);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch { return -1; }
        }

        public bool Update(TeamMember entity)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_TeamMembers_Update", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MemberID", entity.MemberID);
                cmd.Parameters.AddWithValue("@IsCoach", entity.IsCoach);
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
                using var cmd = new SqlCommand("SP_TeamMembers_Remove", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MemberID", id);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }

        public IEnumerable<TeamMember> GetByTeamId(int teamId)
        {
            List<TeamMember> list = new();
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_TeamMembers_GetByTeamID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TeamID", teamId);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read()) list.Add(Map(reader));
            }
            catch { }
            return list;
        }

        public IEnumerable<TeamMemberRosterRowDTO> GetRosterByTeamForPlayer(int teamId, int playerId)
        {
            List<TeamMemberRosterRowDTO> list = new();
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_TeamMembers_GetByPlayer", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TeamID", teamId);
                cmd.Parameters.AddWithValue("@PlayerID", playerId);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read()) list.Add(MapRosterRow(reader));
            }
            catch { }
            return list;
        }

        private static TeamMemberRosterRowDTO MapRosterRow(SqlDataReader reader)
        {
            int ord(string name)
            {
                try { return reader.GetOrdinal(name); }
                catch { return -1; }
            }
            string str(string name)
            {
                int o = ord(name);
                return (o >= 0 && !reader.IsDBNull(o)) ? reader.GetString(o) : string.Empty;
            }
            string? nstr(string name)
            {
                int o = ord(name);
                return (o >= 0 && !reader.IsDBNull(o)) ? reader.GetString(o) : null;
            }

            return new TeamMemberRosterRowDTO
            {
                MemberID = reader.GetInt32(ord("MemberID")),
                TeamID = reader.GetInt32(ord("TeamID")),
                PlayerID = reader.GetInt32(ord("PlayerID")),
                JoinedAt = reader.GetDateTime(ord("JoinedAt")),
                IsCoach = !reader.IsDBNull(ord("IsCoach")) && reader.GetBoolean(ord("IsCoach")),
                FullName = str("FullName"),
                UserName = str("UserName"),
                IsInjured = !reader.IsDBNull(ord("IsInjured")) && reader.GetBoolean(ord("IsInjured")),
                ImagePath = nstr("ImagePath"),
                IsProfileCompleted = !reader.IsDBNull(ord("IsProfileCompleted")) && reader.GetBoolean(ord("IsProfileCompleted")),
                Status = str("Status"),
                JerseyNumber = reader.IsDBNull(ord("JerseyNumber")) ? 0 : reader.GetInt32(ord("JerseyNumber")),
                Position = reader.IsDBNull(ord("Position")) ? string.Empty : reader.GetString(ord("Position"))
            };
        }

        private TeamMember Map(SqlDataReader reader)
        {
            return new TeamMember
            {
                MemberID = reader.GetInt32("MemberID"),
                TeamID = reader.GetInt32("TeamID"),
                PlayerID = reader.GetInt32("PlayerID"),
                JoinedAt = reader.GetDateTime("JoinedAt"),
                IsCoach = reader.GetBoolean("IsCoach")
            };
        }

        public int Create(TeamMember entity)
        {
            throw new NotImplementedException();
        }
    }
}