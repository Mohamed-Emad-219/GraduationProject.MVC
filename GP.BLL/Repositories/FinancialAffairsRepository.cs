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
    public class FinancialAffairsRepository : IFinancialAffairsRepository
    {
        private readonly UserManager<GPUser> userManager;
        private readonly AppDbContext context;
        public FinancialAffairsRepository(UserManager<GPUser> userManager, AppDbContext _context)
        {
            this.userManager = userManager;
            context = _context;
        }
        public FinancialAffairs GetFinancialAffairsByUserId(string UserId)
        {
            return context.FinancialAffairs.FirstOrDefault(f => f.UserId == UserId);
        }
        public async Task<int> UpdateFinancialAffairsAsync(int Id, string Email, string Address, string MobilePhone)
        {
            var faculty = context.FinancialAffairs.FirstOrDefault(f => f.Id == Id);
            if (faculty == null)
            {
                return 0; // not found
            }

            await userManager.SetEmailAsync(faculty.User, Email);
            faculty.Address = Address;
            faculty.MobilePhone = MobilePhone;

            context.FinancialAffairs.Update(faculty);
            return context.SaveChanges(); // returns number of affected rows
        }
    }
}
