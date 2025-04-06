using GP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Interfaces
{
    public interface IFacultyMemberRepsitory
    {
        IEnumerable<FacultyMember> GetHeads();
        IEnumerable<FacultyMember> GetDeans();
        FacultyMember GetDeanByCollegeId(int CollegeId);
        IEnumerable<FacultyMember> GetAll();
        Task<FacultyMember> GetFacultyByUserIdAsync(string UserId);
    }
}
