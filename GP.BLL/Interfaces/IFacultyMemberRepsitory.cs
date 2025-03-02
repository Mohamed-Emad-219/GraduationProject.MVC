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
        IEnumerable<FacultyMember> GetAll();
        FacultyMember GetFacultyByUserId(string UserId);
    }
}
