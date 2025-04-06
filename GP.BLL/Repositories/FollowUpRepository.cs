using GP.BLL.Interfaces;
using GP.DAL.Context;
using GP.DAL.Models;
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
        private readonly AppDbContext context;
        public FollowUpRepository(AppDbContext context)
        {
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
    }
}
