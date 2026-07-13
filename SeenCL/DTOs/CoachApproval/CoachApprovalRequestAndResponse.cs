using Microsoft.AspNetCore.Http;

namespace SeenCL.DTOs
{
    public class CoachApprovalRequestAndResponse
    {
        public int ApprovalID { get; set; }
        public int UserID { get; set; }
        public string Bio { get; set; }
        public IFormFile? CVFile { get; set; } // ??? ??? PDF
    }
}
