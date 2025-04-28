using GP.BLL.Interfaces;
using GP.BLL.ViewModels;
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
        public int GetTotalApplications(int year)
        {
            return _dbContext.Applications.Where(a => a.CreatedAt.Year == year).Count();
        }

        public int GetTotalApproved(int year)
        {
            return _dbContext.Applications.Where(a => a.CreatedAt.Year == year).Count(a => a.Status == Status.Approved);
        }

        public int GetTotalRejected(int year)
        {
            return _dbContext.Applications.Where(a => a.CreatedAt.Year == year).Count(a => a.Status == Status.Rejected);
        }

        public List<CollegeApplicationStats> GetApplicationsByCollege(int year)
        {
            return _dbContext.Applications
                .Where(a => a.Student != null && a.Student.College != null && a.CreatedAt.Year == year)
                .GroupBy(a => a.Student.College.Name)
                .Select(g => new CollegeApplicationStats
                {
                    CollegeName = g.Key,
                    TotalApplications = g.Count(),
                    ApprovedCount = g.Count(a => a.Status == Status.Approved),
                    RejectedCount = g.Count(a => a.Status == Status.Rejected)
                })
                .ToList();
        }

        public GenderDistribution GetGenderDistribution(int year)
        {
            var total = _dbContext.Applications.Count(a => a.CreatedAt.Year == year);
            var male = _dbContext.Applications.Count(a => a.Student != null && a.Student.Gender == Gender.Male && a.CreatedAt.Year == year);
            var female = _dbContext.Applications.Count(a => a.Student != null && a.Student.Gender == Gender.Female && a.CreatedAt.Year == year);

            var malePercentage = total > 0 ? Math.Round((male * 100.0 / total), 2) : 0;
            var femalePercentage = total > 0 ? Math.Round((female * 100.0 / total), 2) : 0;

            return new GenderDistribution
            {
                MalePercentage = malePercentage,
                FemalePercentage = femalePercentage
            };
        }
    }
}
