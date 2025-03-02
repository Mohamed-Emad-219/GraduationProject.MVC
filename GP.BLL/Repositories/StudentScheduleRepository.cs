using GP.BLL.Interfaces;
using GP.DAL.Context;
using GP.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Repositories
{
    public class StudentScheduleRepository : IStudentScheduleRepository
    {
        private readonly AppDbContext context;
        public StudentScheduleRepository(AppDbContext _context)
        {
            context = _context ?? throw new ArgumentNullException(nameof(_context));
        }
        public IEnumerable<StudentSchedule> GetStudentScheduleByGroup(int Group, int Level)
        {
            return context.StudentSchedules
                .Where(s => s.Group == Group && s.Level == Level)
                .Include(s => s.Course)
                .Include(s => s.Place)
                .Include(s => s.Instructor)
                .Include(s => s.Assistant)
                .Include(s => s.Department)
                .ToList();
        }
    }
}
