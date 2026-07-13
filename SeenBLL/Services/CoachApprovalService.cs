using SeenCL.Domain.Entities;
using SeenCL.Domain.Enums;
using SeenCL.DTOs;
using SeenCL.Repositories;
using SeenCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeenBLL.Services
{
    public class CoachApprovalService : ICoachApprovalService
    {
        private readonly ICoachApprovalRepository _repository;
        private readonly IUserRepository _userRepository;

        public CoachApprovalService(ICoachApprovalRepository repository, IUserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public async Task<int> RequestApprovalAsync(CoachApprovalDTO dto)
        {
            var approval = new CoachApproval
            {
                UserID = dto.UserID,
                Bio = dto.Bio,
                CVUrl = dto.CVUrl,
                Status = ApprovalStatus.UnderReview,
                RequestedAt = DateTime.UtcNow
            };
            return await Task.FromResult(_repository.Create(approval));
        }

        public async Task<bool> ProcessApprovalAsync(int approvalId, int adminId, bool accept)
        {
            var approval = await Task.FromResult(_repository.GetById(approvalId));
            if (approval == null) return false;

            approval.Status = accept ? ApprovalStatus.Accepted : ApprovalStatus.Rejected;
            approval.ApprovedByAdminID = adminId;
            approval.ApprovedAt = DateTime.UtcNow;

            bool success = await Task.FromResult(_repository.Update(approval));

            if (success && accept)
            {
                var user = _userRepository.GetById(approval.UserID);
                if (user != null)
                {
                    user.IsCoach = true;
                    _userRepository.Update(user);
                }
            }

            return success;
        }

        public async Task<CoachApprovalDTO?> GetUserRequestAsync(int userId)
        {
            var approval = await Task.FromResult(_repository.GetByUserId(userId));
            return approval != null ? MapToDTO(approval) : null;
        }

        public async Task<IEnumerable<CoachApprovalDTO>> GetAllRequestsAsync()
        {
            var requests = await Task.FromResult(_repository.GetAll());
            return requests.Select(MapToDTO);
        }

        private CoachApprovalDTO MapToDTO(CoachApproval a)
        {
            return new CoachApprovalDTO(
                a.ApprovalID,
                a.UserID,
                a.ApprovedByAdminID,
                (enApprovalStatus)a.Status,
                a.RequestedAt,
                a.ApprovedAt,
                a.Bio,
                a.CVUrl
            );
        }
    }
}
