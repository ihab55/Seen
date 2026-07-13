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
    public class TeamRepository : ITeamRepository
    {
        private readonly IDatabaseHelper _dbHelper;

        public TeamRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        private SqlConnection CreateSqlConnection() => _dbHelper.CreateConnection();

        public Team? GetById(int id)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Teams_GetByID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TeamID", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read()) return Map(reader);
            }
            catch { }
            return null;
        }
        public  PlayerOverviewDTO? GetPlayerOverview(int teamId, int userId)
        {
            PlayerOverviewDTO dto = null;

            using (SqlConnection conn = CreateSqlConnection())
            using (SqlCommand cmd = new SqlCommand("SP_Teams_GetPlayerOverview", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // ✅ Strong typing (better than AddWithValue)
                cmd.Parameters.Add("@TeamID", SqlDbType.Int).Value = teamId;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        dto = new PlayerOverviewDTO
                        {
                            TeamName =  reader["TeamName"].ToString(),
                            CoachName = reader["CoachName"] != DBNull.Value ? reader["CoachName"].ToString() : null,
                            UserRole = reader["UserRole"] != DBNull.Value ? reader["UserRole"].ToString() : null,

                            UserJoinedDate = reader["UserJoinedDate"] != DBNull.Value
                            ? Convert.ToDateTime(reader["UserJoinedDate"])
                            : DateTime.MinValue,

                            PlanName = reader["PlanName"] != DBNull.Value ? reader["PlanName"].ToString() : null,

                            SubscriptionEndDate = reader["SubscriptionEndDate"] != DBNull.Value
                            ? Convert.ToDateTime(reader["SubscriptionEndDate"])
                            : (DateTime?)null,

                            NextTrainingTitle = reader["NextTrainingTitle"] != DBNull.Value ? reader["NextTrainingTitle"].ToString() : null,

                            NextTrainingDate = reader["NextTrainingDate"] != DBNull.Value
                            ? Convert.ToDateTime(reader["NextTrainingDate"])
                            : (DateTime?)null,

                            NextTrainingLocation = reader["NextTrainingLocation"] != DBNull.Value ? reader["NextTrainingLocation"].ToString() : null,

                            UpcomingTrainingsCount = reader["UpcomingTrainingsCount"] != DBNull.Value
                            ? Convert.ToInt32(reader["UpcomingTrainingsCount"])
                            : 0,

                            LastSessionTime = reader["LastSessionTime"] != DBNull.Value
                            ? Convert.ToDateTime(reader["LastSessionTime"])
                            : (DateTime?)null,

                            TotalDistanceKM = reader["TotalDistanceKM"] != DBNull.Value
                            ? Convert.ToDouble(reader["TotalDistanceKM"])
                            : (double?)null,

                            MaxSpeed = reader["MaxSpeed"] != DBNull.Value
                            ? Convert.ToDouble(reader["MaxSpeed"])
                            : (double?)null,

                            AvgHeartRate = reader["AvgHeartRate"] != DBNull.Value
                            ? Convert.ToDouble(reader["AvgHeartRate"])
                            : (double?)null
                        };
                    }
                }
            }

            return dto;
        }

        public IEnumerable<Team> GetAll()
        {
            List<Team> list = new();
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Teams_GetAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read()) list.Add(Map(reader));
            }
            catch { }
            return list;
        }

        public int Create(TeamCreateDTO entity)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Teams_Create", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CoachID", entity.CoachID);
                cmd.Parameters.AddWithValue("@TeamName", entity.TeamName);
                cmd.Parameters.AddWithValue("@TeamCode", entity.TeamCode);
                cmd.Parameters.AddWithValue("@SubscriptionID", entity.SubscriptionID);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch { return -1; }
        }

        public bool Update(Team entity)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Teams_Update", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TeamID", entity.TeamID);
                cmd.Parameters.AddWithValue("@TeamName", entity.TeamName);
                cmd.Parameters.AddWithValue("@SubscriptionID", entity.SubscriptionID);
                cmd.Parameters.AddWithValue("@EndDate", entity.EndDate);
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
                using var cmd = new SqlCommand("SP_Teams_Delete", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TeamID", id);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }

        public Team? GetByCode(string code)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Teams_GetByCode", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TeamCode", code);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read()) return Map(reader);
            }
            catch { }
            return null;
        }

        public IEnumerable<Team> GetByCoachId(int coachId)
        {
            List<Team> list = new();
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Teams_GetByCoachID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CoachID", coachId);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read()) list.Add(Map(reader));
            }
            catch { }
            return list;
        }

        public IEnumerable<PlayerTeamViewDTO> GetByUserId(int userId)
        {
            List<PlayerTeamViewDTO> list = new();
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_Teams_GetByUserID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", userId);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new PlayerTeamViewDTO(
                        reader.GetInt32("TeamID"),
                        reader.GetString("TeamName"),
                        reader.GetString("FirstName"),
                        reader.GetString("LastName"),
                        reader.GetDateTime("Joined")
                    ));
                }
            }
            catch { }
            return list;
        }
        public IEnumerable<CoachTeamListDTO> GetCoachTeams(int coachID)
        {
            List<CoachTeamListDTO> list = new();

            try
            {
                using SqlConnection conn = CreateSqlConnection();

                using SqlCommand cmd =
                    new("SP_Teams_GetByCoachID", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CoachID", coachID);

                conn.Open();

                using SqlDataReader reader =
                    cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new CoachTeamListDTO
                    {
                        TeamID = Convert.ToInt32(reader["TeamID"]),
                        TeamName = reader["TeamName"].ToString()?? string.Empty,
                        TeamCode = reader["TeamCode"].ToString() ?? string.Empty,
                        PlayerCount = Convert.ToInt32(reader["PlayerCount"]),
                        SubscriptionName = reader["SubscriptionName"].ToString() ?? string.Empty,
                        StartDate = Convert.ToDateTime(reader["StartDate"]),
                        EndDate = Convert.ToDateTime(reader["EndDate"]),
                        IsActive = Convert.ToBoolean(reader["IsActive"])
                    });
                }
            }
            catch
            {
                list.Clear();
            }

            return list;
        }

        private Team Map(SqlDataReader reader)
        {
            return new Team
            {
                TeamID = reader.GetInt32("TeamID"),
                CoachID = reader.GetInt32("CoachID"),
                TeamName = reader.GetString("TeamName"),
                TeamCode = reader.GetString("TeamCode"),
                SubscriptionID = reader.GetInt32("SubscriptionID"),
                StartDate = reader.GetDateTime("StartDate"),
                EndDate = reader.GetDateTime("EndDate"),
                IsActive = reader.GetBoolean("IsActive")
            };
        }

        public int Create(Team entity)
        {
            throw new NotImplementedException();
        }
    }
}