using SeenCL.Domain.Entities;
using SeenCL.Interfaces;

namespace SeenCL.Repositories
{
    public interface ICoachApprovalRepository : IRepository<CoachApproval, int>
    {
        CoachApproval? GetByUserId(int userId);
        bool RevokeCoachApproval(int approvalId, int adminId);
        IEnumerable<CoachApproval> GetAll();
    }
}
