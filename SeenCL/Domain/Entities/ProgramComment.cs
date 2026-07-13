using System;

namespace SeenCL.Domain.Entities
{
    public class ProgramComment
    {
        public int CommentID { get; set; }
        public int ProgramID { get; set; }
        public int MemberID { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public int? ParentCommentID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
