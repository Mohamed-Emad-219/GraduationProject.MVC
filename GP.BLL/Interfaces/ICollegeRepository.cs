using GP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Interfaces
{
    public interface ICollegeRepository
    {
        IEnumerable<College> GetColleges();
        Task<College> GetCollegeById(int Id);
        int AddCollege(College college);
        int UpdateCollege(College college);
        Task<int> DeleteCollegeAsync(int Id);
        string GetCollageNameByStudentId(int id);
        int GetCollageIdByStudentId(int id);
    }
}
