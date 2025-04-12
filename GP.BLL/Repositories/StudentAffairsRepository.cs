using GP.BLL.Interfaces;
using GP.DAL.Context;
using GP.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Repositories
{
    public class StudentAffairsRepository : IStudentAffairsRepository
    {
        private readonly UserManager<GPUser> userManager;
        private readonly AppDbContext context;
        public StudentAffairsRepository(UserManager<GPUser> userManager, AppDbContext _context)
        {
            this.userManager = userManager;
            context = _context;
        }
        public StudentAffairs GetStudentAffairsByUserId(string UserId)
        {
            return context.StudentAffairs.FirstOrDefault(f => f.UserId == UserId);
        }
        public async Task<int> UpdateStudentAffairsAsync(int Id, string Email, string Address, string MobilePhone)
        {
            var faculty = context.StudentAffairs.FirstOrDefault(f => f.Id == Id);
            if (faculty == null)
            {
                return 0; // not found
            }

            await userManager.SetEmailAsync(faculty.User, Email);
            faculty.Address = Address;
            faculty.MobilePhone = MobilePhone;

            context.StudentAffairs.Update(faculty);
            return context.SaveChanges(); // returns number of affected rows
        }

    }
}
