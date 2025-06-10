using GP.BLL.Interfaces;
using GP.BLL.Repositories;
using GP.DAL.Context;
using GP.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Controllers.Student
{
    [Authorize(Roles = "Student")]

    public class StudentController : Controller
    {
        private readonly IStudentScheduleRepository _studentScheduleRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly UserManager<GPUser> _userManager;
        public StudentController(UserManager<GPUser> userManager, IStudentRepository studentRepository, IStudentScheduleRepository studentScheduleRepository)
        {
            _studentScheduleRepository = studentScheduleRepository;
            _studentRepository = studentRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> StudentSchedule()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;
            var std = _studentRepository.GetStudentByUserId(userId);
            ViewData["Schedule"] = _studentScheduleRepository
                .GetStudentScheduleByGroup(std.Group, std.Level, std.DeptId);
            ViewData["group"] = std.Group;
            return View();
        }
    }
}
