using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GP.BLL.ViewModels;
using GP.DAL.Models;

namespace GP.BLL.Interfaces
{
    public interface IStudentDistribution
    {
        public Task<int> totalNumberofStudents(int year, SemesterType semester);
        public Task<int> totalNumberofDepartment();
        public Task<double> AVGStudentperdepartment(int year, SemesterType semester);
        public IEnumerable<Department> GetAllDepartments();
        Task<List<Departmentsvm>> GetStudentsPerDepartment(int year, SemesterType semester);
        public Task<KeyValuePair<string, int>> HighestEnrollments(int year, SemesterType semester);
        public Task<KeyValuePair<string, int>> LowestEnrollments(int year, SemesterType semester);
    }
}
