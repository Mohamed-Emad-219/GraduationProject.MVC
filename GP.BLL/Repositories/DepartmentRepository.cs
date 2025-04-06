using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GP.BLL.Interfaces;
using GP.DAL.Context;
using GP.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GP.BLL.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _dbContext;

        public DepartmentRepository( AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int AddDepartment(Department department)
        {
            _dbContext.Add(department);
            return  _dbContext.SaveChanges();
        }

        public int DeleteDepartmentAsync(int Id)
        {
            
            var dep = GetDepartmentById(Id);
            _dbContext.Remove(dep);
            return _dbContext.SaveChanges();
        }

        public Department GetDepartmentById(int Id)
        {
            var dep = _dbContext.Departments.Find(Id);//// find op search in cache if found return it else search in database
            
            return dep;
        }

        public IEnumerable<Department> GetDepartments()
        {
           var result=_dbContext.Departments.Include(d => d.College).AsNoTracking().ToList();
            return result;
        }

        public Department GetDepartmentByName(string Name)
        {
            return _dbContext.Departments.FirstOrDefault(d => d.Name == Name);
        }
        public int UpdateDepartment(Department department)
        {
            _dbContext.Departments.Update(department);
            return _dbContext.SaveChanges();
        }
        public async Task<List<Department>> GetDepartmentsByCollegeIdAsync(int Id)
        {
            var departments = await _dbContext.Departments
                                            .Where(d => d.CollegeId == Id)
                                            .ToListAsync();
            await Task.Delay(1000);
            return departments;
        }
    }
}
