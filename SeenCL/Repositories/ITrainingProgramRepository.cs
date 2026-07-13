using SeenCL.Domain.Entities;
using SeenCL.Interfaces;
using System.Collections.Generic;

namespace SeenCL.Repositories
{

    public interface ITrainingProgramRepository : IRepository<TrainingProgram, int>
    {
        IEnumerable<TrainingProgram> GetByTeamId(int teamId);
    }
}
