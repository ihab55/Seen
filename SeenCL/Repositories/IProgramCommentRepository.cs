using SeenCL.Domain.Entities;
using SeenCL.Interfaces;

namespace SeenCL.Repositories
{
    public interface IProgramCommentRepository : IRepository<ProgramComment, int>
    {
        IEnumerable<ProgramComment> GetByProgramId(int programId);
    }
}
