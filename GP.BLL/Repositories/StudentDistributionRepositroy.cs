using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GP.BLL.Interfaces;
using GP.BLL.ViewModels;
using GP.DAL.Context;
using GP.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GP.BLL.Repositories
{
    public class StudentDistributionRepositroy : IStudentDistribution
    {
        private readonly AppDbContext _dbcontext;

        public StudentDistributionRepositroy(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<double> AVGStudentperdepartment(int year)
        {
            var totalStudents = await _dbcontext.Enrollments
        .Where(e => e.Term.AcademicYear == year)
        .Select(e => e.Student)
        .CountAsync();

            var totalDepartments = await _dbcontext.Departments.CountAsync();

            if (totalDepartments == 0)
                return 0; // avoid division by zero

            return (double)totalStudents / totalDepartments;
        }

        public async Task<int> totalNumberofDepartment()
        {
            return await _dbcontext.Departments.CountAsync();
        }

        public async Task<int> totalNumberofStudents(int year)
        {
            return await _dbcontext.Enrollments.Where(e => e.Term.AcademicYear == year).Select(e => e.StudentId).CountAsync();
        }
        public IEnumerable<Department> GetAllDepartments()
        {
            return _dbcontext.Departments.ToList();
        }
        public async Task<int> GetTotalStudents()
        {
            return await _dbcontext.Departments.SumAsync(d => d.Students.Count);
        }

        //public double GetAverageStudentsPerDepartment(int year)
        //{
        //    int totalStudents = GetTotalStudents();
        //    int departmentCount = _dbcontext.Departments.Count();
        //    return departmentCount > 0 ? (double)totalStudents / departmentCount : 0;
        //}
        public async Task<List<Departmentsvm>> GetStudentsPerDepartment(int year)
        {
            // Get total number of students for the year (only once)
            var totalStudents = await _dbcontext.Enrollments
                .Where(e => e.Term.AcademicYear == year)
                .Select(e => e.StudentId) // use StudentId for performance
                .CountAsync();

            // Group enrollments by department and count them
            var departmentStats = await _dbcontext.Departments
                .Select(d => new Departmentsvm
                {
                    DepartmentName = d.Name,
                    NumberOfStudents = _dbcontext.Enrollments
                        .Where(e => e.Term.AcademicYear == year && e.Student.DeptId == d.Id)
                        .Select(e => e.StudentId) // again, use StudentId instead of full entity
                        .Distinct()
                        .Count(), // ⚠️ this is still problematic, needs to be evaluated below
                    Percentage = 0 // Placeholder, calculate later
                })
                .ToListAsync();

            // Now calculate percentage in-memory to avoid complex EF translation issues
            foreach (var dept in departmentStats)
            {
                dept.Percentage = totalStudents == 0
                    ? 0
                    : (double)dept.NumberOfStudents / totalStudents * 100;
            }

            return departmentStats;
        }

        public async Task<KeyValuePair<string, int>> HighestEnrollments(int year)
        {
            var deps = await GetStudentsPerDepartment(year);

            if (deps == null || deps.Count == 0)
                return new KeyValuePair<string, int>("N/A", 0);

            var topDep = deps.OrderByDescending(d => d.NumberOfStudents).First();

            return new KeyValuePair<string, int>(topDep.DepartmentName, topDep.NumberOfStudents);
        }


        public async Task<KeyValuePair<string, int>> LowestEnrollments(int year)
        {
            var deps = await GetStudentsPerDepartment(year);

            if (deps == null || deps.Count == 0)
                return new KeyValuePair<string, int>("N/A", 0);

            var topDep = deps.OrderBy(d => d.NumberOfStudents).First();

            return new KeyValuePair<string, int>(topDep.DepartmentName, topDep.NumberOfStudents);
        }

    }

}
