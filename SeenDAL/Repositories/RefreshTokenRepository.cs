using Microsoft.Data.SqlClient;
using SeenCL.Domain.Entities;
using SeenCL.Repositories;
using SeenDAL.Infrastructure;
using System;
using System.Data;

namespace SeenDAL.Repositories
{
    /// <summary>
    /// Handles persistence of JWT refresh tokens using SQL Server stored procedures.
    /// </summary>
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IDatabaseHelper _dbHelper;

        public RefreshTokenRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        private SqlConnection CreateSqlConnection() => _dbHelper.CreateConnection();

        /// <summary>
        /// Persists a new refresh token. Returns the generated TokenID, or -1 on failure.
        /// </summary>
        public int Create(RefreshToken entity)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_RefreshTokens_Create", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID",    (object?)entity.UserID    ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@AdminID",   (object?)entity.AdminID   ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Token",     entity.Token);
                cmd.Parameters.AddWithValue("@ExpiresAt", entity.ExpiresAt);

                conn.Open();
                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Retrieves a refresh token record by its string value.
        /// Returns null if not found.
        /// </summary>
        public RefreshToken? GetByToken(string token)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_RefreshTokens_GetByToken", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Token", token);

                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (!reader.Read()) return null;

                return Map(reader);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Marks a refresh token as revoked. Returns true if the update was applied.
        /// </summary>
        public bool Revoke(string token)
        {
            try
            {
                using var conn = CreateSqlConnection();
                using var cmd = new SqlCommand("SP_RefreshTokens_Revoke", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Token", token);

                conn.Open();
                var affected = cmd.ExecuteScalar();
                return affected != null && Convert.ToInt32(affected) > 0;
            }
            catch
            {
                return false;
            }
        }

        private static RefreshToken Map(SqlDataReader reader)
        {
            return new RefreshToken
            {
                TokenID   = reader.GetInt32(reader.GetOrdinal("TokenID")),
                UserID    = reader.IsDBNull(reader.GetOrdinal("UserID"))  ? null : (int?)reader.GetInt32(reader.GetOrdinal("UserID")),
                AdminID   = reader.IsDBNull(reader.GetOrdinal("AdminID")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("AdminID")),
                Token     = reader.GetString(reader.GetOrdinal("Token")),
                ExpiresAt = reader.GetDateTime(reader.GetOrdinal("ExpiresAt")),
                IsRevoked = reader.GetBoolean(reader.GetOrdinal("IsRevoked")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
            };
        }
    }
}
