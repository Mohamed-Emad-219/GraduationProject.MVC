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

    }
}
