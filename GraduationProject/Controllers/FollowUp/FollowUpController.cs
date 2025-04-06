using GP.BLL.Interfaces;
using GP.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace GraduationProject.Controllers.FollowUp
{
    [Authorize(Roles = "FollowUp")]
    public class FollowUpController : Controller
    {
        private readonly IFollowUpRepository followUpRepository;
        private readonly UserManager<GPUser> userManager;
        public FollowUpController(IFollowUpRepository followUpRepository, UserManager<GPUser> _userManager)
        {
            this.followUpRepository = followUpRepository;
            userManager = _userManager;
        }
        public IActionResult FollowUpSchedule()
        {
            return View();
        }
        public async Task<IActionResult> GetDayTable(string day)
        {
            if (string.IsNullOrEmpty(day))
            {
                return BadRequest("Day is required.");
            }
            var data = followUpRepository.GetFollowUpData(day);
            var user = await userManager.GetUserAsync(User);
            var followup = followUpRepository.GetFollowUpByUserId(user.Id);
            ViewData["User"] = followup;
            return PartialView("_FollowUpTablePartial", data);
        }
        [HttpPost]
        public async Task<IActionResult> SubmitFollowUp(Dictionary<int, Dictionary<int, bool>> attended, int FollowUpId)
        {
            var followUpEntries = new List<FollowUpEntry>();

            // Check if 'attended' is not null and contains any data
            if (attended != null)
            {
                foreach (var entry in attended)
                {
                    int followUpEntryId = entry.Key; // FollowUpEntryId

                    foreach (var week in entry.Value)
                    {
                        // Retrieve the FollowUpEntry from the repository
                        var followUpEntry = await followUpRepository.GetFollowUpEntryAsync(followUpEntryId, FollowUpId);

                        if (followUpEntry != null)
                        {
                            // Check if the checkbox was checked or unchecked (true/false)
                            followUpEntry.Attended = week.Value;
                            followUpEntry.WeekNumber = week.Key; // Week number (1-14)

                            // Add to list of updated FollowUpEntries
                            followUpEntries.Add(followUpEntry);
                        }
                    }
                }
            }

            // After looping through the data, update the FollowUpEntries in the repository
            if (followUpEntries.Any())
            {
                await followUpRepository.UpdateFollowUpEntriesAsync(followUpEntries);
            }

            // Redirect after the update is complete
            return RedirectToAction("Index", "Home");
        }

    }

}