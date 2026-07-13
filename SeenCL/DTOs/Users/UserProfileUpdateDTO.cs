using Microsoft.AspNetCore.Http;

namespace SeenCL.DTOs
{
    public class UserProfileUpdateDTO
    {
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public double? FateRate { get; set; }
        public string? ImagePath { get; set; }
        public IFormFile? ImageFile { get; set; }
    }

}
