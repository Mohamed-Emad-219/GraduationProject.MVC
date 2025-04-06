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
    public class PetitionRequestRepository : IPetitionRequestRepository
    {
        private readonly AppDbContext context;
        public PetitionRequestRepository(AppDbContext _context)
        {
            context = _context;
        }
        public int AddPetition(PetitionRequest PetitionRequest)
        {
            context.PetitionRequests.Add(PetitionRequest);
            return context.SaveChanges();
        }
        public IEnumerable<PetitionRequest> GetPetitionRequests()
        {
            return context.PetitionRequests.Include(d => d.College).AsNoTracking().ToList();
        }
        public PetitionRequest GetPetitionRequestById(int Id)
        {
            return context.PetitionRequests.Include(p => p.College)
                .Include(p => p.Department)
                .Include(p => p.Dean)
                .Where(f => f.Dean.WorkingHours == 3)
                .AsNoTracking()
                .FirstOrDefault(p => p.Id == Id);
        }
        
    }
}
