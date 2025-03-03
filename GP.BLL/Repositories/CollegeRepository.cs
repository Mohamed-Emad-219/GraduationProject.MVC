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
    public class CollegeRepository : ICollegeRepository
    {
        private readonly AppDbContext _dbContext;

        public CollegeRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<College> GetColleges()
        {
            var result = _dbContext.Colleges.AsNoTracking().ToList();
            return result;
        }
        public async Task<College> GetCollegeById(int Id)
        {
            var college = await _dbContext.Colleges.FindAsync(Id);//// find op search in cache if found return it else search in database
            return college;
        }
        public string GetCollageNameByStudentId(int id)
        {
            return _dbContext.Students
        .Where(s => s.Id == id)
        .Select(s => s.Department.College.Name)
        .FirstOrDefault();
        }
        public int GetCollageIdByStudentId(int id)
        {
            return _dbContext.Students
        .Where(s => s.Id == id)
        .Select(s => s.Department.College.Id)
        .FirstOrDefault();
        }
        public int AddCollege(College college)
        {
            _dbContext.Add(college);
            return _dbContext.SaveChanges();
        }
        public int UpdateCollege(College college)
        {
            _dbContext.Colleges.Update(college);
            return _dbContext.SaveChanges();
        }
        public async Task<int> DeleteCollegeAsync(int Id) { 
            var college = await GetCollegeById(Id);
            _dbContext.Remove(college);
            return _dbContext.SaveChanges();
        }
    }
}
