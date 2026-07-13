namespace SeenCL.DTOs
{
    public class CommentDTO
    {
        public int CommentID { get; set; }
        public int? ParentCommentID { get; set; }
        public string CommentText { get; set; }
        public string FullName { get; set; }
        public string? ImagePath { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        // ????? ?????? ??????? ???? ???????
        public List<CommentDTO> Replies { get; set; } = new List<CommentDTO>();

    }
}
