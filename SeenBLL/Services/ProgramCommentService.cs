using SeenCL.Domain.Entities;
using SeenCL.DTOs;
using SeenCL.Repositories;
using SeenCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeenBLL.Services
{
    public class ProgramCommentService : IProgramCommentService
    {
        private readonly IProgramCommentRepository _repository;

        public ProgramCommentService(IProgramCommentRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> AddCommentAsync(ProgramCommentDTO dto)
        {
            var comment = new ProgramComment
            {
                ProgramID = dto.ProgramID,
                MemberID = dto.MemberID,
                CommentText = dto.CommentText,
                CreatedAt = DateTime.UtcNow
            };
            return await Task.FromResult(_repository.Create(comment));
        }

        public async Task<IEnumerable<ProgramCommentDTO>> GetCommentsForProgramAsync(int programId)
        {
            var comments = await Task.FromResult(_repository.GetByProgramId(programId));
            return comments.Select(c => new ProgramCommentDTO(
                c.CommentID,
                c.ProgramID,
                c.MemberID,
                c.CommentText,
                null, // ParentCommentID
                false, // IsDeleted
                c.CreatedAt
            ));
        }
    }
}
