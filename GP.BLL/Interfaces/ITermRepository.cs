using GP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Interfaces
{
    public interface ITermRepository
    {
        Term GetTermById(int Id);
        Term GetTermByLevel(int level);
        Term GetTermByDetails(int level, SemesterType semester, int academicYear);
        Term GetTermBySemesterYear(SemesterType semester, int academicYear);
    }
}
