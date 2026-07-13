namespace SeenCL.DTOs
{
    public enum enApprovalStatus { UnderReview = 0, Accepted = 1, Rejected = 2 }
    public class CoachApprovalDTO
    {
        public int ApprovalID { get; set; }
        public int UserID { get; set; }
        public int? ApprovedByAdminID { get; set; }
        public enApprovalStatus Status { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string Bio { get; set; } = string.Empty;
        public string CVUrl { get; set; } = string.Empty;

        // Constructor ?????? ???????? ???????
        public CoachApprovalDTO(int approvalID, int userID, int? approvedByAdminID,
                               enApprovalStatus status, DateTime requestedAt,
                               DateTime? approvedAt, string bio, string cvUrl)
        {
            ApprovalID = approvalID;
            UserID = userID;
            ApprovedByAdminID = approvedByAdminID;
            Status = status;
            RequestedAt = requestedAt;
            ApprovedAt = approvedAt;
            Bio = bio;
            CVUrl = cvUrl;
        }
    }
}
