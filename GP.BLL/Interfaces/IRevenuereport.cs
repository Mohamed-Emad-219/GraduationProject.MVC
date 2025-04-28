using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GP.DAL.Models;

namespace GP.BLL.Interfaces
{
    public interface IRevenuereport
    {
        public List<Receipt> GetAllReceipts(DateTime Date);

    }
}
