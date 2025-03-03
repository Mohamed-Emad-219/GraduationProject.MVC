using GP.BLL.Interfaces;
using GP.DAL.Context;
using GP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Repositories
{
    public class StudentAffairsRepository : IStudentAffairsRepository
    {
        private readonly AppDbContext context;
        public StudentAffairsRepository(AppDbContext _context)
        {
            context = _context;
        }
        public StudentAffairs GetStudentAffairsByUserId(string UserId)
        {
            return context.StudentAffairs.FirstOrDefault(f => f.UserId == UserId);
        }
        //public StudentAffairs GetManagerByUserId(string UserId)
        //{
        //    return context.StudentAffairs.FirstOrDefault(f => f.UserId == UserId && f.ManagerId != null);
        //}

    }
}
