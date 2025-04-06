using GP.BLL.Interfaces;
using GP.DAL.Context;
using GP.DAL.Models;
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

        public FacultyMemberRepsitory(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<FacultyMember> GetHeads()
        {
            return _dbContext.Departments.Where(d => d.HeadId.HasValue).Select(d => d.Head).ToList();
        }
        public IEnumerable<FacultyMember> GetDeans()
        {
            return _dbContext.Colleges.Where(d => d.DeanId.HasValue).Select(d => d.Dean).ToList();
        }
        public IEnumerable<FacultyMember> GetAll()
        {
            return _dbContext.FacultyMembers.AsNoTracking().ToList();
        }
        public async Task<FacultyMember> GetFacultyByUserIdAsync(string UserId)
        {
            return await _dbContext.FacultyMembers
                           .FirstOrDefaultAsync(f => f.UserId == UserId);
        }
        public FacultyMember GetDeanByCollegeId(int CollegeId)
        {
            return _dbContext.Colleges.Where(f => f.DeanId.HasValue && f.Id == CollegeId).Select(c => c.Dean).FirstOrDefault();
        }
        
    }
}
