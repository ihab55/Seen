using System;

namespace SeenCL.Domain.Entities
{
    /// <summary>
    /// Represents a persisted refresh token for JWT token renewal.
    /// Supports both User and Admin refresh sessions.
    /// </summary>
    public class RefreshToken
    {
        public int TokenID { get; set; }

        /// <summary>Nullable: set when the token belongs to a regular User.</summary>
        public int? UserID { get; set; }

        /// <summary>Nullable: set when the token belongs to an Admin.</summary>
        public int? AdminID { get; set; }

        /// <summary>The opaque, cryptographically random refresh token string.</summary>
        public string Token { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
