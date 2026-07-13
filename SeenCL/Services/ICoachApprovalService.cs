using SeenCL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeenCL.Services
{
    public interface ICoachApprovalService
    {
        Task<int> RequestApprovalAsync(CoachApprovalDTO dto);
        Task<bool> ProcessApprovalAsync(int approvalId, int adminId, bool accept);
        Task<CoachApprovalDTO?> GetUserRequestAsync(int userId);
        Task<IEnumerable<CoachApprovalDTO>> GetAllRequestsAsync();
    }
}
