using GP.BLL.Interfaces;
using GP.DAL.Context;
using GP.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Repositories
{
    public class AdvisorRepository : IAdvisorRepository
    {
        private readonly AppDbContext context;
        private readonly UserManager<GPUser> _userManager;
        public AdvisorRepository(UserManager<GPUser> userManager, AppDbContext _context)
        {
            context = _context;
            _userManager = userManager;
        }
        public Advisor GetAdvisorByUserId(string UserId)
        {
            return context.Advisors.FirstOrDefault(f => f.UserId == UserId);
        }
        public async Task<int> UpdateAdvisorAsync(int Id, string Email, string Address, string MobilePhone)
        {
            var faculty = context.Advisors.FirstOrDefault(f => f.Id == Id);
            if (faculty == null)
            {
                return 0; // not found
            }

            await _userManager.SetEmailAsync(faculty.User, Email);
            faculty.Address = Address;
            faculty.MobilePhone = MobilePhone;

            context.Advisors.Update(faculty);
            return context.SaveChanges(); // returns number of affected rows
        }
    }
}
