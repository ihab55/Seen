using SeenCL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeenCL.Services
{
    public interface ITrainingProgramService
    {
        Task<int> CreateProgramAsync(TrainingProgramDTO dto);
        Task<IEnumerable<TrainingProgramDTO>> GetTeamProgramsAsync(int teamId);
    }
}
