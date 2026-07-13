using SeenCL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeenCL.Services
{
    public interface IProgramCommentService
    {
        Task<int> AddCommentAsync(ProgramCommentDTO dto);
        Task<IEnumerable<ProgramCommentDTO>> GetCommentsForProgramAsync(int programId);
    }
}
