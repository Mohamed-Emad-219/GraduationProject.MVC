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
    public class AdminRepository : IAdminRepository
    {
        private readonly UserManager<GPUser> _userManager;
        private readonly AppDbContext context;
        public AdminRepository(UserManager<GPUser> userManager, AppDbContext _context)
        {
            this._userManager = userManager;
            context = _context;
        }
        public Admin GetAdminByUserId(string UserId)
        {
            return context.Admins.FirstOrDefault(f => f.UserId == UserId);
        }
        public async Task<int> UpdateAdminAsync(int Id, string Email, string Address, string MobilePhone)
        {
            var faculty = context.Admins.FirstOrDefault(f => f.Id == Id);
            if (faculty == null)
            {
                return 0; // not found
            }

            await _userManager.SetEmailAsync(faculty.User, Email);
            faculty.Address = Address;
            faculty.MobilePhone = MobilePhone;

            context.Admins.Update(faculty);
            return context.SaveChanges(); // returns number of affected rows
        }
    }
}
