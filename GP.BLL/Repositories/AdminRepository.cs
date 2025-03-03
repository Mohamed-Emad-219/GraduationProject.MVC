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
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext context;
        public AdminRepository(AppDbContext _context)
        {
            context = _context;
        }
        public Admin GetAdminByUserId(string UserId)
        {
            return context.Admins.FirstOrDefault(f => f.UserId == UserId);
        }
    }
}
