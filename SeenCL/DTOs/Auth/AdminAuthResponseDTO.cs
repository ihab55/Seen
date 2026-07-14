using System;

namespace SeenCL.DTOs.Auth
{
    /// <summary>
    /// Returned on successful Admin login.
    /// Contains both the short-lived JWT access token and the long-lived refresh token.
    /// </summary>
    public class AdminAuthResponseDTO
    {
        /// <summary>Short-lived JWT Bearer token to include in Authorization header.</summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>Long-lived opaque refresh token for getting a new access token.</summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>UTC expiry time of the access token.</summary>
        public DateTime AccessTokenExpiresAt { get; set; }

        /// <summary>UTC expiry time of the refresh token.</summary>
        public DateTime RefreshTokenExpiresAt { get; set; }

        /// <summary>Admin ID.</summary>
        public int AdminID { get; set; }

        /// <summary>Admin display name.</summary>
        public string AdminName { get; set; } = string.Empty;

        /// <summary>Admin email.</summary>
        public string Email { get; set; } = string.Empty;
    }
}
