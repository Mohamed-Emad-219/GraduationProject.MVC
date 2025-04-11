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
    public class PetitionCourseRepository : IPetitionCourseRepository
    {
        private readonly AppDbContext _dbContext;

        public PetitionCourseRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int AddPetitionCourse(PetitionCourse petitionCourse)
        {
            _dbContext.PetitionCourses.Add(petitionCourse);
            return _dbContext.SaveChanges();
        }
        public IEnumerable<PetitionCourse> GetPetitionCoursesByPetitionId(int Id)
        {
            return _dbContext.PetitionCourses
                .Where(p => p.PetitionRequestId == Id)
                .Include(p => p.Course)
                .ToList();
        }
        public PetitionCourse GetPetitionCourseByPetitionCourseId(int Id)
        {
            return _dbContext.PetitionCourses
                .Include(p => p.PetitionRequest)
                .FirstOrDefault(p => p.Id == Id);
        }
    }
}
