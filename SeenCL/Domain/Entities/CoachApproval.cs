using System;
using SeenCL.Domain.Enums;

namespace SeenCL.Domain.Entities
{
    public class CoachApproval
    {
        public int ApprovalID { get; set; }
        public int UserID { get; set; }
        public int? ApprovedByAdminID { get; set; }
        public ApprovalStatus Status { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string Bio { get; set; } = string.Empty;
        public string CVUrl { get; set; } = string.Empty;
    }
}
