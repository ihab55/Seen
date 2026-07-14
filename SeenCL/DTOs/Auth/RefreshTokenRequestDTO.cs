namespace SeenCL.DTOs.Auth
{
    /// <summary>
    /// Request body for refreshing an expired access token using a valid refresh token.
    /// </summary>
    public class RefreshTokenRequestDTO
    {
        /// <summary>The opaque refresh token previously issued at login.</summary>
        public string RefreshToken { get; set; } = string.Empty;
    }
}
