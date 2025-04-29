using GP.BLL.Interfaces;
using GP.DAL.Context;
using GP.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Repositories
{
    public class FacultyMemberRepsitory : IFacultyMemberRepsitory
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<GPUser> _userManager;
        public FacultyMemberRepsitory(UserManager<GPUser> userManager, AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public IEnumerable<FacultyMember> GetHeads()
        {
            return _dbContext.Departments.Where(d => !string.IsNullOrEmpty(d.HeadId)).Select(d => d.Head).ToList();
        }
        public IEnumerable<FacultyMember> GetDeans()
        {
            return _dbContext.Colleges.Where(d => !string.IsNullOrEmpty(d.DeanId)).Select(d => d.Dean).ToList();
        }
        public IEnumerable<FacultyMember> GetAll()
        {
            return _dbContext.FacultyMembers.AsNoTracking().ToList();
        }
        public IEnumerable<FacultyMember> GetInstructorsAssistants()
        {
            return _dbContext.FacultyMembers.Where(f=>f.WorkingHours == 6 || f.WorkingHours == 28).ToList();
        }
        public string GetInstructorNameByCourseCode(string courseCode)
        {
            return _dbContext.FacultyMembers.Where(f => f.TeacherId.Contains("I") && f.CourseInstructors.FirstOrDefault().CourseCode == courseCode).Select(f=>f.FullName).FirstOrDefault();
        }
        public async Task<FacultyMember> GetFacultyByUserIdAsync(string UserId)
        {
            return await _dbContext.FacultyMembers
                           .FirstOrDefaultAsync(f => f.UserId == UserId);
        }
        public FacultyMember GetDeanByCollegeId(int CollegeId)
        {
            return _dbContext.Colleges.Where(f => !string.IsNullOrEmpty(f.DeanId) && f.Id == CollegeId).Select(c => c.Dean).FirstOrDefault();
        }
        public async Task<int> UpdateFacultyAsync(string Id, string Email, string Address, string MobilePhone)
        {
            var faculty = _dbContext.FacultyMembers.FirstOrDefault(f => f.TeacherId == Id);
            if (faculty == null)
            {
                return 0; // not found
            }

            await _userManager.SetEmailAsync(faculty.User, Email);
            faculty.Address = Address;
            faculty.MobilePhone = MobilePhone;

           _dbContext.FacultyMembers.Update(faculty);
            return await _dbContext.SaveChangesAsync(); // returns number of affected rows
        }
        public IEnumerable<FacultyMember> GetInstructorsAssistantsByDep(int? DeptId)
        {
            return _dbContext.FacultyMembers.Where(f => (f.WorkingHours == 6 || f.WorkingHours == 28)&& f.DeptId == DeptId).ToList();

        }
    }
}
