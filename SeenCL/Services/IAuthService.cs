using SeenCL.DTOs;
using SeenCL.DTOs.Auth;

namespace SeenCL.Services
{
    /// <summary>
    /// Defines authentication operations: login, token refresh, and token revocation.
    /// Separated from IUserService to follow the Single Responsibility Principle.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a User by username/email + password.
        /// Returns an AuthResponseDTO containing JWT access token and refresh token, or null if credentials are invalid.
        /// </summary>
        Task<AuthResponseDTO?> LoginAsync(UserLoginDTO loginDto);

        /// <summary>
        /// Authenticates an Admin by email + password.
        /// Returns an AdminAuthResponseDTO containing JWT access token (with Admin role claim) and refresh token.
        /// </summary>
        Task<AdminAuthResponseDTO?> AdminLoginAsync(AdminLoginRequestDTO loginDto);

        /// <summary>
        /// Exchanges a valid, non-expired, non-revoked refresh token for a new access token.
        /// Also rotates the refresh token (issues a new one and revokes the old one).
        /// </summary>
        Task<AuthResponseDTO?> RefreshTokenAsync(string refreshToken);

        /// <summary>
        /// Revokes a refresh token to immediately invalidate a session (logout).
        /// </summary>
        Task<bool> RevokeTokenAsync(string refreshToken);
    }
}
