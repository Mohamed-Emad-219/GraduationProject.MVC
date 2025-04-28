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
            var month = DateTime.Now.Month;
            SemesterType semester;

            if (month >= 9 || month <= 1)
            {
                semester = SemesterType.Fall;
            }
            else if (month >= 2 && month <= 6)
            {
                semester = SemesterType.Spring;
            }
            else
            {
                semester = SemesterType.Fall;
            }
            var schedules = context.StudentSchedules
                .Where(s => s.Level == level && s.Semester == semester)
                .Include(s => s.FacultyMember)
                .Include(s => s.Course)
                .Include(s => s.Place)
                .ToList(); // load from DB

            //Console.WriteLine(schedules.Where(s => IsGroupMatch(s.Group, group)).ToList());
            return schedules.Where(s => IsGroupMatch(s.Group, group)).ToList(); // apply C# filter
        }

        private bool IsGroupMatch(string dbGroup, string searchGroup)
        {
            if (string.IsNullOrEmpty(dbGroup) || string.IsNullOrEmpty(searchGroup))
                return false;

            searchGroup = searchGroup.Trim();

            // Direct exact match (e.g., "G1" matches "G1")
            if (dbGroup.Trim().Equals(searchGroup, StringComparison.OrdinalIgnoreCase))
                return true;

            // Check if dbGroup is a range like "G1 to G4" (e.g., "G1 to G4" matches "G2")
            if (dbGroup.Contains("to", StringComparison.OrdinalIgnoreCase))
            {
                var parts = dbGroup.Split("to", StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    var start = parts[0].Trim();
                    var end = parts[1].Trim();

                    if (start.StartsWith('G') && end.StartsWith('G'))
                    {
                        // Remove the "G" and parse the numbers
                        if (int.TryParse(start.Substring(1), out int startNum) &&
                            int.TryParse(end.Substring(1), out int endNum) &&
                            int.TryParse(searchGroup.Substring(1), out int searchNum))
                        {
                            // Check if the search group is within the range
                            return searchNum >= startNum && searchNum <= endNum;
                        }
                    }
                }
            }

            // Check if dbGroup is a list of groups like "G1, G2, G3"
            var groupList = dbGroup.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                   .Select(g => g.Trim());
            //Console.WriteLine($"Group list: {string.Join(", ", groupList)}");
            // Match if the searchGroup is in the list (e.g., "G1" matches "G1, G2, G3")
            return groupList.Any(g => g.Equals(searchGroup, StringComparison.OrdinalIgnoreCase));
        }
    }
}
