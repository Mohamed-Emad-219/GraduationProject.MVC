using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GP.BLL.Interfaces;
using GP.DAL.Context;
using GP.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace GP.BLL.Repositories
{
    public class InstructorScheduleRepositroy : IInstructorScheduleRepositroy
    {
        private readonly AppDbContext _dbContext;

        public InstructorScheduleRepositroy(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<InstructorSchedule> GetInstructorScheduleByInstructorId(int InstructorId)
        {
            return _dbContext.InstructorSchedules
    .Where(s => s.InstructorId == InstructorId) // Filter only instructor schedules
    .Include(s => s.Course)
    .Include(s => s.Place)
    .Include(s => s.Instructor)
    .ToList();

        }
        public IEnumerable<InstructorSchedule> GetAssistantScheduleByAssistantId(int AssistantId)
        {
            return _dbContext.InstructorSchedules
    .Where(s => s.AssistantId == AssistantId) // Filter only assistant schedules
    .Include(s => s.Course)
    .Include(s => s.Place)
    .Include(s => s.Assistant)
    .ToList();
        }
    }
}
