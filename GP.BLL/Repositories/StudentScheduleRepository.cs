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
        public IEnumerable<StudentSchedule> GetStudentScheduleByGroup(string? group, int? level, int? depId)
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
            Department major = context.Departments.FirstOrDefault(d => d.Id == depId);
            var studentMajor = GetMajorAbbreviation(major.Name);
            Console.WriteLine(studentMajor);
            foreach (var g in schedules.Where(s => IsGroupMatch(s.Group, group, studentMajor, level)).ToList()) Console.WriteLine(g.Course.CourseName);
            var scheduless = schedules.Where(s => IsGroupMatch(s.Group, group, studentMajor, level)).ToList(); // apply C# filter
            return scheduless;
        }
        private string GetMajorAbbreviation(string fullMajorName)
        {
            return fullMajorName switch
            {
                "Artificial Intelligence" => "AI",
                "Computer Science" => "CS",
                "Information Systems" => "IS",
                _ => fullMajorName // fallback to original if not matched
            };
        }
        private bool IsGroupMatch(string dbGroup, string searchGroup, string studentMajor,int? level)
        {
            if (string.IsNullOrEmpty(dbGroup) || string.IsNullOrEmpty(searchGroup) || string.IsNullOrEmpty(studentMajor))
                return false;

            searchGroup = NormalizeGroup(searchGroup);

            // Direct match with major
            if (NormalizeGroup(dbGroup).Equals(searchGroup, StringComparison.OrdinalIgnoreCase) &&
                (ExtractMajor(dbGroup).Equals(studentMajor, StringComparison.OrdinalIgnoreCase) || studentMajor == "General"))
                return true;
            
            // Range match
            if (dbGroup.Contains("to", StringComparison.OrdinalIgnoreCase) || dbGroup.Length == 2 || dbGroup.Length == 3)
            {
                var parts = dbGroup.Split("to", StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    var start = NormalizeGroup(parts[0]);
                    var end = NormalizeGroup(parts[1]);
                    var rangeMajor = ExtractMajor(parts[0]); // Assume both start and end have same major

                    if (start.StartsWith('G') && end.StartsWith('G') && searchGroup.StartsWith('G'))
                    {
                        if (int.TryParse(start.Substring(1), out int startNum) &&
                            int.TryParse(end.Substring(1), out int endNum) &&
                            int.TryParse(searchGroup.Substring(1), out int searchNum))
                        {
                            return (searchNum >= startNum &&
                                   searchNum <= endNum &&
                                   rangeMajor.Equals(studentMajor, StringComparison.OrdinalIgnoreCase)) || 
                                   (searchNum >= startNum &&
                                   searchNum <= endNum && (level == 1 || level == 2));
                        }
                    }
                }
            }

            // List match with major
            var groupList = dbGroup.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                   .Select(g => g.Trim());
            //Console.WriteLine(groupList);

            foreach (var g in groupList)
            {
                if (NormalizeGroup(g).Equals(searchGroup, StringComparison.OrdinalIgnoreCase) &&
                    ExtractMajor(g).Equals(studentMajor, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        // Removes specialization suffix, e.g., "G2 (AI)" -> "G2"
        private string NormalizeGroup(string group)
        {
            int index = group.IndexOf('(');
            if (index > 0)
            {
                group = group.Substring(0, index).Trim();
            }
            return group.Trim();
        }

        // Extracts specialization suffix, e.g., "G2 (AI)" -> "AI"
        private string ExtractMajor(string group)
        {
            int start = group.IndexOf('(');
            int end = group.IndexOf(')');
            if (start >= 0 && end > start)
            {
                return group.Substring(start + 1, end - start - 1).Trim();
            }
            return string.Empty;
        }
    }
}
