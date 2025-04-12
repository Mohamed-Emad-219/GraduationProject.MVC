using GP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Interfaces
{
    public interface IStudentAffairsRepository
    {
        StudentAffairs GetStudentAffairsByUserId(string UserId);
        Task<int> UpdateStudentAffairsAsync(int Id, string Email, string Address, string MobilePhone);

    }
}
