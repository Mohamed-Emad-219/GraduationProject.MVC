using GP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Interfaces
{
    public interface IFollowUpRepository
    {
        FollowUp GetFollowUpByUserId(string UserId);
        IEnumerable<FollowUpEntry> GetFollowUpData(string day);
        Task<IEnumerable<FollowUpEntry>> GetFollowUpEntriesAsyncById(int followUpId);
        Task UpdateFollowUpEntriesAsync(IEnumerable<FollowUpEntry> followUpEntries);
        Task<FollowUpEntry> GetFollowUpEntryAsync(int followUpEntryId, int followUpId);
    }
}
