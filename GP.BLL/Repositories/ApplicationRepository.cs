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
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly AppDbContext _dbContext;

        public ApplicationRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int AddApplication(Application App)
        {
            _dbContext.Add(App);
            return _dbContext.SaveChanges();
        }
        public IEnumerable<Application> GetApplications()
        {
            return _dbContext.Applications.Include(a => a.Student).ThenInclude(a => a.College).ToList();
        }
        public Application GetApplicationById(int id)
        {
            return _dbContext.Applications.FirstOrDefault(a => a.Id == id);
        }
        public void UpdateApplication(Application app)
        {
            _dbContext.Update(app);
            _dbContext.SaveChanges();
        }
    }
}
