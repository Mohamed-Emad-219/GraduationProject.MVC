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
    public class AdvisorRepository : IAdvisorRepository
    {
        private readonly AppDbContext context;
        public AdvisorRepository(AppDbContext _context)
        {
            context = _context;
        }
        public Advisor GetAdvisorByUserId(string UserId)
        {
            return context.Advisors.FirstOrDefault(f => f.UserId == UserId);
        }
    }
}
