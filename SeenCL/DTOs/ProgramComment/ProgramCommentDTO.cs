namespace SeenCL.DTOs
{
    public class ProgramCommentDTO
    {
        public int CommentID { get; set; }
        public int ProgramID { get; set; }
        public int MemberID { get; set; }
        public string CommentText { get; set; }
        public int? ParentCommentID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }

        // ???? ?????? ?? ??? Join ???? ?????
        public string FullName { get; set; }
        public string? ImagePath { get; set; }

        public ProgramCommentDTO(int CommentID, int ProgramID, int MemberID, string CommentText,
            int? ParentCommentID, bool IsDeleted, DateTime CreatedAt)
        {
            this.CommentID = CommentID;
            this.ProgramID = ProgramID;
            this.MemberID = MemberID;
            this.CommentText = CommentText;
            this.ParentCommentID = ParentCommentID;
            this.IsDeleted = IsDeleted;
            this.CreatedAt = CreatedAt;
        }
    }
}
