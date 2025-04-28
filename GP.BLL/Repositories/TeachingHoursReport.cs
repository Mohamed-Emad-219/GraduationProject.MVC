using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GP.BLL.Interfaces;
using GP.BLL.ViewModels;
using GP.DAL.Context;
using GP.DAL.Models;
using GraduationProject.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace GP.BLL.Repositories
{
    public class TeachingHoursReport : ITeachingHoursReport
    {
        private readonly AppDbContext _dbContext;

        public TeachingHoursReport(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public List<CourseDisplayVM> GetCoursesDean(SemesterType semester, int year)
        {
            return _dbContext.CoursesTerms
                .Where(ct => ct.Term.Semester == semester && ct.Term.AcademicYear == year)
                .SelectMany(ct => ct.Course.CourseInstructors.Select(ci => new CourseDisplayVM
                {
                    FacultyName = ci.FacultyMember.FullName, // in case multiple instructors
                    CourseCode = ci.Course.Code,
                    CourseName = ci.Course.CourseName,
                    Credits = ci.Course.CreditHour,
                    WeeklyHours = ci.Course.CreditHour
                }))
                .ToList();
        }
        public List<CourseDisplayVM> GetCoursesHead(int? depid,SemesterType semester, int year)
        {
            return _dbContext.CoursesTerms
                .Where(ct => ct.Term.Semester == semester && ct.Term.AcademicYear == year && ct.Course.DeptId == depid)
                .SelectMany(ct => ct.Course.CourseInstructors.Select(ci => new CourseDisplayVM
                {
                    FacultyName = ci.FacultyMember.FullName, // in case multiple instructors
                    CourseCode = ci.Course.Code,
                    CourseName = ci.Course.CourseName,
                    Credits = ci.Course.CreditHour,
                    WeeklyHours = ci.Course.CreditHour
                }))
                .ToList();
        }
        public List<FacultyMember> GetFacultyMembersDean()
        {

            var facultyDetails = _dbContext.FacultyMembers
                //.Select(fm => new TeachingHoursVM
                //{
                //    Name = fm.FirstName + " " + fm.MiddleName + " " + fm.LastName,
                //    WorkingHours = fm.WorkingHours,
                //    Department = fm.Department.Name
                    
                //})
                .Include(fm => fm.Department)
                .Where(f => f.TeacherId.Contains("TA") || f.TeacherId.Contains("I"))
               .ToList();
            return facultyDetails;
        }
        public List<FacultyMember> GetFacultyMembersHead(int? depid)
        {
            var facultyDetails = _dbContext.FacultyMembers
                .Where(f => f.DeptId == depid)
                .Include(f=>f.Department)
                //.Select(fm => new TeachingHoursVM
                //{
                //    Name = "Dr. " + fm.FirstName + " " + fm.MiddleName + " " + fm.LastName,
                //    TotalHours = fm.WorkingHours,
                //    OverloadHours = fm.WorkingHours > 9 ? fm.WorkingHours - 9 : 0
                //})
                
               .ToList();
            return facultyDetails;
        }
        public int OverloadTeachingHours()
        {
            return (int)_dbContext.FacultyMembers
                .Where(fm => fm.WorkingHours > 9)
                .Sum(fm => fm.WorkingHours - 9);
        }

        public int TotalFacultyMembers()
        {
            return _dbContext.FacultyMembers.Count();
        }

        public int TotalTeachingHoursAssigned()
        {
            return (int)_dbContext.FacultyMembers
                .Where(fm => fm.WorkingHours > 0)
                .Sum(fm => fm.WorkingHours);// without overload hours--
        }
    }


}
