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
    public class TrainingProgramService : ITrainingProgramService
    {
        private readonly ITrainingProgramRepository _repository;

        public TrainingProgramService(ITrainingProgramRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateProgramAsync(TrainingProgramDTO dto)
        {
            var program = new TrainingProgram
            {
                TeamID = dto.TeamID,
                TeamMemberID = dto.TeamMemberID,
                ProgramName = dto.ProgramName,
                Goal = dto.Goal,
                IntensityLevel = (byte)dto.IntensityLevel,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = (SeenCL.Domain.Enums.ProgramStatus)dto.Status,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow
            };
            return await Task.FromResult(_repository.Create(program));
        }

        public async Task<IEnumerable<TrainingProgramDTO>> GetTeamProgramsAsync(int teamId)
        {
            var programs = await Task.FromResult(_repository.GetByTeamId(teamId));
            return programs.Select(p => new TrainingProgramDTO(
                p.ProgramID,
                p.TeamID,
                p.TeamMemberID,
                p.ProgramName,
                p.Goal,
                p.IntensityLevel,
                p.StartDate,
                p.EndDate,
                (enProgramStatus)p.Status,
                p.Notes,
                p.CreatedAt
            ));
        }
    }
}
