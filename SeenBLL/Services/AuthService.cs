using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SeenCL.Domain.Entities;
using SeenCL.DTOs;
using SeenCL.DTOs.Auth;
using SeenCL.Repositories;
using SeenCL.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SeenBLL.Services
{
    /// <summary>
    /// Handles JWT authentication for Users and Admins.
    /// Responsibilities:
    ///   - Validate credentials using BCrypt password verification.
    ///   - Issue signed JWT access tokens with role claims (User, Coach, Admin).
    ///   - Issue and persist opaque refresh tokens.
    ///   - Rotate refresh tokens on use (old token revoked, new one issued).
    ///   - Support token revocation (logout).
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IConfiguration _configuration;

        // JWT settings loaded from appsettings.json
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _accessTokenExpiryMinutes;
        private readonly int _refreshTokenExpiryDays;

        public AuthService(
            IUserRepository userRepository,
            IAdminRepository adminRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IConfiguration configuration)
        {
            _userRepository           = userRepository;
            _adminRepository          = adminRepository;
            _refreshTokenRepository   = refreshTokenRepository;
            _configuration            = configuration;

            _secretKey                = _configuration["JwtSettings:SecretKey"]
                                        ?? throw new InvalidOperationException("JwtSettings:SecretKey is not configured.");
            _issuer                   = _configuration["JwtSettings:Issuer"] ?? "SeenAPI";
            _audience                 = _configuration["JwtSettings:Audience"] ?? "SeenClient";
            _accessTokenExpiryMinutes = int.TryParse(_configuration["JwtSettings:AccessTokenExpiryMinutes"], out var exp) ? exp : 60;
            _refreshTokenExpiryDays   = int.TryParse(_configuration["JwtSettings:RefreshTokenExpiryDays"],   out var rExp) ? rExp : 7;
        }

        // ──────────────────────────────────────────────────────────────────────
        // PUBLIC API
        // ──────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Validates User credentials and returns a full AuthResponseDTO on success.
        /// The JWT claims the role "Coach" if IsCoach == true, otherwise "User".
        /// </summary>
        public async Task<AuthResponseDTO?> LoginAsync(UserLoginDTO loginDto)
        {
            var user = await Task.FromResult(_userRepository.GetByEmailOrUsername(loginDto.UserName));
            if (user == null) return null;

            // BCrypt verification — compares plain-text password against the stored hash
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                return null;

            string role            = user.IsCoach ? "Coach" : "User";
            var    accessExpiry    = DateTime.UtcNow.AddMinutes(_accessTokenExpiryMinutes);
            var    refreshExpiry   = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            string accessToken     = GenerateJwtToken(user.UserID.ToString(), user.UserName, user.Email, role, accessExpiry);
            string refreshTokenStr = GenerateSecureRefreshToken();

            PersistRefreshToken(userId: user.UserID, adminId: null, refreshTokenStr, refreshExpiry);

            return new AuthResponseDTO
            {
                AccessToken           = accessToken,
                RefreshToken          = refreshTokenStr,
                AccessTokenExpiresAt  = accessExpiry,
                RefreshTokenExpiresAt = refreshExpiry,
                UserInfo = new UserResponseDTO
                {
                    UserID             = user.UserID,
                    FullName           = $"{user.FirstName} {user.LastName}",
                    Email              = user.Email,
                    IsProfileCompleted = user.IsProfileCompleted,
                    Height             = user.Height,
                    Weight             = user.Weight,
                    FateRate           = user.FateRate,
                    IsCoach            = user.IsCoach,
                    ImagePath          = user.ImagePath
                }
            };
        }

        /// <summary>
        /// Validates Admin credentials and returns an AdminAuthResponseDTO with an Admin-role JWT on success.
        /// </summary>
        public async Task<AdminAuthResponseDTO?> AdminLoginAsync(AdminLoginRequestDTO loginDto)
        {
            var admin = await Task.FromResult(_adminRepository.GetByEmail(loginDto.Email));
            if (admin == null) return null;

            // BCrypt verification
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, admin.PasswordHash))
                return null;

            var    accessExpiry    = DateTime.UtcNow.AddMinutes(_accessTokenExpiryMinutes);
            var    refreshExpiry   = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            string accessToken     = GenerateJwtToken(admin.AdminID.ToString(), admin.AdminName, admin.Email, "Admin", accessExpiry);
            string refreshTokenStr = GenerateSecureRefreshToken();

            PersistRefreshToken(userId: null, adminId: admin.AdminID, refreshTokenStr, refreshExpiry);

            return new AdminAuthResponseDTO
            {
                AccessToken           = accessToken,
                RefreshToken          = refreshTokenStr,
                AccessTokenExpiresAt  = accessExpiry,
                RefreshTokenExpiresAt = refreshExpiry,
                AdminID               = admin.AdminID,
                AdminName             = admin.AdminName,
                Email                 = admin.Email
            };
        }

        /// <summary>
        /// Exchanges a valid refresh token for a new access token.
        /// Implements refresh token rotation: the old token is revoked and a new one is issued.
        /// Returns null if the token is expired, revoked, or not found.
        /// </summary>
        public async Task<AuthResponseDTO?> RefreshTokenAsync(string refreshToken)
        {
            var tokenRecord = await Task.FromResult(_refreshTokenRepository.GetByToken(refreshToken));

            if (tokenRecord == null || tokenRecord.IsRevoked || tokenRecord.ExpiresAt < DateTime.UtcNow)
                return null;

            // Only User refresh tokens are handled here (Admin refresh uses the same flow internally)
            if (tokenRecord.UserID == null) return null;

            var user = await Task.FromResult(_userRepository.GetById(tokenRecord.UserID.Value));
            if (user == null) return null;

            // Rotate: revoke old token and issue fresh ones
            _refreshTokenRepository.Revoke(refreshToken);

            string role            = user.IsCoach ? "Coach" : "User";
            var    accessExpiry    = DateTime.UtcNow.AddMinutes(_accessTokenExpiryMinutes);
            var    refreshExpiry   = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            string newAccessToken     = GenerateJwtToken(user.UserID.ToString(), user.UserName, user.Email, role, accessExpiry);
            string newRefreshTokenStr = GenerateSecureRefreshToken();

            PersistRefreshToken(userId: user.UserID, adminId: null, newRefreshTokenStr, refreshExpiry);

            return new AuthResponseDTO
            {
                AccessToken           = newAccessToken,
                RefreshToken          = newRefreshTokenStr,
                AccessTokenExpiresAt  = accessExpiry,
                RefreshTokenExpiresAt = refreshExpiry,
                UserInfo = new UserResponseDTO
                {
                    UserID             = user.UserID,
                    FullName           = $"{user.FirstName} {user.LastName}",
                    Email              = user.Email,
                    IsProfileCompleted = user.IsProfileCompleted,
                    Height             = user.Height,
                    Weight             = user.Weight,
                    FateRate           = user.FateRate,
                    IsCoach            = user.IsCoach,
                    ImagePath          = user.ImagePath
                }
            };
        }

        /// <summary>
        /// Revokes a refresh token immediately (used for logout).
        /// </summary>
        public async Task<bool> RevokeTokenAsync(string refreshToken)
        {
            return await Task.FromResult(_refreshTokenRepository.Revoke(refreshToken));
        }

        // ──────────────────────────────────────────────────────────────────────
        // PRIVATE HELPERS
        // ──────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Generates a signed JWT access token with standard and custom claims.
        /// </summary>
        private string GenerateJwtToken(string id, string name, string email, string role, DateTime expiry)
        {
            var key         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,   id),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Name,  name),
                new Claim(ClaimTypes.Role,               role),
                new Claim(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,   DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var token = new JwtSecurityToken(
                issuer:             _issuer,
                audience:           _audience,
                claims:             claims,
                notBefore:          DateTime.UtcNow,
                expires:            expiry,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Generates a cryptographically secure, URL-safe refresh token string (64 bytes → 88 Base64 chars).
        /// </summary>
        private static string GenerateSecureRefreshToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Persists a refresh token entity to the database.
        /// </summary>
        private void PersistRefreshToken(int? userId, int? adminId, string token, DateTime expiry)
        {
            var entity = new RefreshToken
            {
                UserID    = userId,
                AdminID   = adminId,
                Token     = token,
                ExpiresAt = expiry,
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow
            };
            _refreshTokenRepository.Create(entity);
        }
    }
}
