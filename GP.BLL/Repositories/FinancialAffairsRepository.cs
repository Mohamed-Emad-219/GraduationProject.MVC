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
    public class FinancialAffairsRepository : IFinancialAffairsRepository
    {
        private readonly AppDbContext context;
        public FinancialAffairsRepository(AppDbContext _context)
        {
            context = _context;
        }
        public FinancialAffairs GetFinancialAffairsByUserId(string UserId)
        {
            return context.FinancialAffairs.FirstOrDefault(f => f.UserId == UserId);
        }
        //public FinancialAffairs GetManagerByUserId(string UserId)
        //{
        //    return context.FinancialAffairs.FirstOrDefault(f => f.UserId == UserId && f.ManagerId != null);
        //}
    }
}
