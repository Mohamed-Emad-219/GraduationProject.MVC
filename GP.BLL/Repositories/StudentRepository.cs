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
    }
}
