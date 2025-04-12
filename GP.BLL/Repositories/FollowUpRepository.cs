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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GP.BLL.Repositories
{
    public class FollowUpRepository : IFollowUpRepository
    {
        private readonly UserManager<GPUser> userManager;
        private readonly AppDbContext context;
        public FollowUpRepository(UserManager<GPUser> userManager, AppDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }

        public IEnumerable<FollowUpEntry> GetFollowUpData(string day)
        {
            var f = context.FollowUpEntries
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Course)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Instructor)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Assistant)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Place)
                .Where(f => f.Schedule.Day == day)
                .ToList();
            return f;
        }
        public async Task<IEnumerable<FollowUpEntry>> GetFollowUpEntriesAsyncById(int followUpId)
        {
            return await context.FollowUpEntries
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Course)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Instructor)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Assistant)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Place)
                .Where(f => f.FollowUpId == followUpId)
                .ToListAsync();
        }

        public async Task UpdateFollowUpEntriesAsync(IEnumerable<FollowUpEntry> followUpEntries)
        {
            context.FollowUpEntries.UpdateRange(followUpEntries);
            await context.SaveChangesAsync();
        }

        public async Task<FollowUpEntry> GetFollowUpEntryAsync(int followUpEntryId, int followUpId)
        {
            return await context.FollowUpEntries
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Course)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Instructor)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Place)
                .FirstOrDefaultAsync(f => f.Id == followUpEntryId && f.FollowUpId == followUpId);
        }
        public FollowUp GetFollowUpByUserId(string UserId)
        {
            return context.FollowUps.FirstOrDefault(f => f.UserId == UserId);
        }
        public async Task<int> UpdateFollowUpAsync(int Id, string Email, string Address, string MobilePhone)
        {
            var faculty = context.FollowUps.FirstOrDefault(f => f.Id == Id);
            if (faculty == null)
            {
                return 0; // not found
            }

            await userManager.SetEmailAsync(faculty.User, Email);
            faculty.Address = Address;
            faculty.MobilePhone = MobilePhone;

            context.FollowUps.Update(faculty);
            return context.SaveChanges(); // returns number of affected rows
        }
    }
}
