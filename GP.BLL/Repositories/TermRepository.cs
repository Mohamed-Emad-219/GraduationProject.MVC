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
    public class TermRepository : ITermRepository
    {
        private readonly AppDbContext context;
        public TermRepository(AppDbContext _context)
        {
            context = _context;
        }
        public int AddTerm(Term term)
        {
            context.Add(term);
            return context.SaveChanges();
        }
        public IEnumerable<Term> GetTerms()
        {
            return context.Terms.ToList();
        }
        public Term GetTermById(int Id)
        {
            return context.Terms.FirstOrDefault(t => t.Id == Id);
        }
        public Term GetTermByLevel(int level)
        {
            return context.Terms.LastOrDefault(t => t.Level == level);
        }
        public Term GetTermByDetails(int level, SemesterType semester, int academicYear)
        {
            return context.Terms
                       .FirstOrDefault(t => t.Level == level &&
                                            t.Semester == semester &&
                                            t.AcademicYear == academicYear);
        }
        public Term GetTermBySemesterYear(SemesterType semester, int academicYear)
        {
            Console.WriteLine($"Searching for Semester: {(int)semester}, AcademicYear: {academicYear}");

            var term = context.Terms
                .Include(t => t.Enrollments)                // Load enrollments
                    .ThenInclude(e => e.Course)             // Load course details
                    .ThenInclude(c => c.InstructorSchedules) // Load course instructor schedules
                    .ThenInclude(i => i.FacultyMember)
                .Include(t => t.Enrollments)                // Load enrollments again
                    .ThenInclude(e => e.Student)            // Load student details
                .FirstOrDefault(t => t.Semester == semester &&
                                     t.AcademicYear == academicYear);

            if (term == null)
            {
                Console.WriteLine("No matching term found in the database.");
            }
            else
            {
                Console.WriteLine($"Found Term: {term.Id}, Semester: {term.Semester}, Year: {term.AcademicYear}");
            }

            return term;
        }

    }
}
