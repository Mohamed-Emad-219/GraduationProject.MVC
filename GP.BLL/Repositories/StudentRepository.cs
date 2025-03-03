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
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext context;
        public StudentRepository(AppDbContext _context)
        {
            context = _context;
        }
        public Student GetStudentByUserId(string UserId)
        {
            return context.Students.FirstOrDefault(f => f.UserId == UserId);
        }
        public Student GetStudentById(int Id)
        {
            return context.Students.FirstOrDefault(f => f.Id == Id);
        }
        public IEnumerable<Student> GetStudentsByLevel(int level)
        {
            return context.Students
                       .Where(s => s.Level == level)
                       .ToList();
        }

    }
}
