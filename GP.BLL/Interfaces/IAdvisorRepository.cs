using GP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Interfaces
{
    public interface IAdvisorRepository
    {
        Advisor GetAdvisorByUserId(string UserId);
        Task<int> UpdateAdvisorAsync(int Id, string Email, string Address, string MobilePhone);
    }
}
