using GP.BLL.Interfaces;
using GP.DAL.Context;
using GP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Repositories
{
    public class ResultPetitionRepository :IResultPetitionRepository
    {
        private readonly AppDbContext context;
        private readonly IPetitionCourseRepository petitionCourseRepository;
        public ResultPetitionRepository(IPetitionCourseRepository _petitionCourseRepository, AppDbContext _context)
        {
            context = _context;
            petitionCourseRepository = _petitionCourseRepository;
        }
        public int AddResultPetition(ResultPetition result)
        {
            context.ResultPetitions.Add(result);
            var petitioncourse = petitionCourseRepository
                .GetPetitionCourseByPetitionCourseId(result.PetitionCourseId);
            petitioncourse.PetitionRequest.Status = "Done";
            return context.SaveChanges();
        }
    }
}
