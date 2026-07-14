using SeenCL.Domain.Entities;

namespace SeenCL.Repositories
{
    /// <summary>
    /// Repository contract for persisting and validating JWT refresh tokens.
    /// </summary>
    public interface IRefreshTokenRepository
    {
        /// <summary>Persists a new refresh token record. Returns the generated TokenID.</summary>
        int Create(RefreshToken entity);

        /// <summary>Retrieves a refresh token record by its token string value.</summary>
        RefreshToken? GetByToken(string token);

        /// <summary>Marks a refresh token as revoked (soft-invalidate for logout/rotation).</summary>
        bool Revoke(string token);
    }
}
