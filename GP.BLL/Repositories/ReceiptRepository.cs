using GP.BLL.Interfaces;
using GP.DAL.Context;
using GP.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly AppDbContext context;
        public ReceiptRepository(AppDbContext context)
        {
            this.context = context;
        }

        public int AddReceipt(Receipt receipt)
        {
            context.Receipts.Add(receipt);
            return context.SaveChanges();
        }
        public IEnumerable<Receipt> GetReceipts()
        {
            return context.Receipts.Include(r => r.Student).ToList();
        }
        public Receipt GetReceiptWithStudentById(int id)
        {
            return context.Receipts.Include(r=>r.Student).ThenInclude(r=>r.College).FirstOrDefault(r => r.Id == id);
        }
        public Receipt GetReceiptWithFinancialById(int id)
        {
            return context.Receipts.Include(r => r.FinancialAffairs).FirstOrDefault(r => r.Id == id);
        }
        public void UpdateReceiptStudentAffairId(int Id, int StudentAffairsId)
        {
            var receipt = context.Receipts.FirstOrDefault(r => r.Id == Id);
            if (receipt != null)
            {
                receipt.StudentAffairsId = StudentAffairsId;
                context.SaveChanges();
            }
        }
    }
}
