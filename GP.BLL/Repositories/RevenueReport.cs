using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GP.BLL.Interfaces;
using GP.DAL.Context;
using GP.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GP.BLL.Repositories
{
    public class RevenueReport : IRevenuereport
    {
        private readonly AppDbContext _dbContext;

        public RevenueReport( AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Receipt> GetAllReceipts(DateTime Date)
        {
            return _dbContext.Receipts
        .Include(r => r.Student)        // Optional: if you need Student info
        .Include(r => r.College)         // Optional: if you need College info
        .Where(r => r.PaymentDate == Date)
        .ToList();
        }
    }
   
}
