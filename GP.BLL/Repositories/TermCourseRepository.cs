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
    public class TermCourseRepository : ITermCourseRepository
    {
        private readonly AppDbContext context;
        public TermCourseRepository ( AppDbContext _context)
        {
            context = _context;
        }
        public int AddCourseTerm(CoursesTerm coursesTerm)
        {
            context.CoursesTerms.Add(coursesTerm);
            return context.SaveChanges();
        }
        public int GetCoursePrice(string courseCode, int termId)
        {
            var courseTerm = context.CoursesTerms
            .FirstOrDefault(ct => ct.CourseCode == courseCode && ct.TermId == termId);

            return courseTerm?.Price ?? 0;
        }
        public IEnumerable<Course> GetCoursesPerTerm(int Id, int? Level, int? DeptId)
        {
            return context.CoursesTerms
                  .Where(c => c.TermId == Id && c.Course.Level == Level && c.Course.DeptId == DeptId)
                  .Select(s => s.Course)
                  .ToList();
        }
    }
}
