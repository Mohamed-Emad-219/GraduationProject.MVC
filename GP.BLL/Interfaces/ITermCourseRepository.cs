using GP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Interfaces
{
    public interface ITermCourseRepository
    {
        int GetCoursePrice(string courseCode, int termId);
        IEnumerable<Course> GetCoursesPerTerm(int Id);
    }
}
