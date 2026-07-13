namespace SeenCL.DTOs
{
    public class UserResponseDTO
    {
        public int UserID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsProfileCompleted { get; set; }
        public int? Height { get; set; }

        public int? Weight { get; set; }

        public double? FateRate { get; set; }
        public bool IsCoach { get; set; }
        public string? ImagePath { get; set; }
        public byte[]? ImageData { get; set; }
    }

}
