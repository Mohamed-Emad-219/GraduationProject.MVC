using GP.BLL.Interfaces;
using GP.DAL.Context;
using GP.DAL.Dto;
using GP.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Repositories
{
    public class PlaceRepository : IPlaceRepository
    {
        private readonly AppDbContext context;
        public PlaceRepository(AppDbContext _context)
        {
            context = _context;
        }
        public IEnumerable<Place> GetAll()
        {
            return context.Places.AsNoTracking().ToList();
        }
        public IEnumerable<PlaceDTO> GetPlacesNameId()
        {
            return context.Places
        .Select(c => new PlaceDTO { Value = c.PlaceId, Text = c.PlaceName }) // Avoid circular references
        .ToList();
        }
    }
}
