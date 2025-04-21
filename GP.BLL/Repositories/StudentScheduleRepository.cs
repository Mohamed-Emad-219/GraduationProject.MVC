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
        public IEnumerable<StudentSchedule> GetStudentScheduleByGroup(string? group, int? level)
        {
            var schedules = context.StudentSchedules
                .Where(s => s.Level == level)
                .Include(s => s.Course)
                .Include(s => s.Place)
                .Include(s => s.FacultyMember)
                .ToList(); // load from DB

            return schedules.Where(s => IsGroupMatch(s.Group, group)).ToList(); // apply C# filter
        }

        private bool IsGroupMatch(string dbGroup, string searchGroup)
        {
            if (string.IsNullOrEmpty(dbGroup) || string.IsNullOrEmpty(searchGroup))
                return false;

            // If exactly matches
            if (dbGroup == searchGroup)
                return true;

            // Check if dbGroup is a range like "G1 to G4"
            if (dbGroup.Contains("to"))
            {
                var parts = dbGroup.Split("to", StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    var start = parts[0].Trim(); // "G1"
                    var end = parts[1].Trim();   // "G4"

                    // Remove the "G" and parse numbers
                    if (int.TryParse(start.Substring(1), out int startNum) &&
                        int.TryParse(end.Substring(1), out int endNum) &&
                        int.TryParse(searchGroup.Substring(1), out int searchNum))
                    {
                        // Check if searchNum is between startNum and endNum
                        return searchNum >= startNum && searchNum <= endNum;
                    }
                }
            }

            // Otherwise, simple contains
            return dbGroup.Contains(searchGroup);
        }
    }
}
