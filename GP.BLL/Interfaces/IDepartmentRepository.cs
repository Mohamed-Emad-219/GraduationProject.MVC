using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GP.DAL.Models;

namespace GP.BLL.Interfaces
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department>GetDepartments();
        Task<Department> GetDepartmentById(int Id);
        Department GetDepartmentByName(string Name);
        int AddDepartment(Department department);
        int UpdateDepartment(Department department);
        Task<int> DeleteDepartmentAsync(int Id);

    }
}
