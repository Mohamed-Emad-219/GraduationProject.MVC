using GP.BLL.Interfaces;
using GP.BLL.Repositories;
using GP.DAL.Context;
using GP.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Controllers.Instructor
{
    public class InstructorController : Controller
    {
     //   private readonly AppDbContext _context;
        private readonly IInstructorScheduleRepositroy _instructorScheduleRepositroy;
        private readonly IFacultyMemberRepsitory _facultyMemberRepsitory;
        private readonly UserManager<GPUser> _userManager;

        public InstructorController(IFacultyMemberRepsitory facultyMemberRepsitory, UserManager<GPUser> userManager, IInstructorScheduleRepositroy instructorScheduleRepositroy)
        {
            //_context = context; // Dependency Injection
            _instructorScheduleRepositroy = instructorScheduleRepositroy;
            _userManager = userManager;
            _facultyMemberRepsitory = facultyMemberRepsitory;
        }
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> InstructorSchedule()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;
            var inst = _facultyMemberRepsitory.GetFacultyByUserId(userId);
            ViewData["Schedule"] = _instructorScheduleRepositroy.GetInstructorScheduleByInstructorId(inst.Id);
            return View();
        }
        [Authorize(Roles = "Assistant")]

        public async Task<IActionResult> AssistantSchedule()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;
            var assistant = _facultyMemberRepsitory.GetFacultyByUserId(userId);
            ViewData["Schedule"] = _instructorScheduleRepositroy.GetAssistantScheduleByAssistantId(assistant.Id);
            return View();
        }
    }
}
