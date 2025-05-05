using GP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Interfaces
{
    public interface IReceiptRepository
    {
        int AddReceipt(Receipt receipt);
        IEnumerable<Receipt> GetReceipts();
        IEnumerable<Receipt> GetReceiptsStudentAffairsNullOnly();
        Receipt GetReceiptWithStudentById(int id);
        Receipt GetReceiptWithFinancialById(int id);
        void UpdateReceiptStudentAffairId(int Id, int StudentAffairsId);
    }
}
