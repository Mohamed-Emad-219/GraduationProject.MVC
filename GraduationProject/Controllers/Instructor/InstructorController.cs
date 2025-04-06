using GP.BLL.Interfaces;
using GP.BLL.Repositories;
using GP.DAL.Context;
using GP.DAL.Models;
using GraduationProject.Controllers.Advisor;
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
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ICollegeRepository _collegeRepository;
        private readonly IEnrollmentRepository enrollmentRepository;

        public InstructorController(IEnrollmentRepository _enrollmentRepository, ICollegeRepository collegeRepository, IDepartmentRepository departmentRepository, IFacultyMemberRepsitory facultyMemberRepsitory, UserManager<GPUser> userManager, IInstructorScheduleRepositroy instructorScheduleRepositroy)
        {
            //_context = context; // Dependency Injection
            _instructorScheduleRepositroy = instructorScheduleRepositroy;
            _userManager = userManager;
            _facultyMemberRepsitory = facultyMemberRepsitory;
            _departmentRepository = departmentRepository;
            _collegeRepository = collegeRepository;
            enrollmentRepository = _enrollmentRepository;
        }
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> InstructorSchedule()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;
            var inst = _facultyMemberRepsitory.GetFacultyByUserIdAsync(userId);
            ViewData["Schedule"] = _instructorScheduleRepositroy.GetInstructorScheduleByInstructorId(inst.Id);
            return View();
        }
        [Authorize(Roles = "Assistant")]

        public async Task<IActionResult> AssistantSchedule()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;
            var assistant = _facultyMemberRepsitory.GetFacultyByUserIdAsync(userId);
            ViewData["Schedule"] = _instructorScheduleRepositroy.GetAssistantScheduleByAssistantId(assistant.Id);
            return View();
        }
        [Authorize(Roles = "Dean")]
        public async Task<IActionResult> CourseEnrollmentReport()
        {
            var user = await _userManager.GetUserAsync(User);
            var dean = await _facultyMemberRepsitory.GetFacultyByUserIdAsync(user.Id);
            await Task.Delay(1000);
            var collegeid = _collegeRepository.GetCollegeIdByDeanId(dean.Id);
            ViewData["Department"] = await _departmentRepository.GetDepartmentsByCollegeIdAsync(collegeid);
            //await Task.Delay(1000);
            return View();
        }
        public async Task<IActionResult> SearchEnroll(string CourseCode, int DeptId, int CollegeId, SemesterType Semester, int AcademicYear)
        {
            ViewData["Department"] = _departmentRepository.GetDepartmentById(DeptId);
            if (ViewData["Department"] == null)
            {
                return NotFound("Department not found.");
            }

            var user = await _userManager.GetUserAsync(User);
            var dean = await _facultyMemberRepsitory.GetFacultyByUserIdAsync(user.Id);
            await Task.Delay(1000);
            ViewData["College"] = _collegeRepository.GetCollegeByDeanId(dean.Id);
            ViewData["Semester"] = Semester.ToString();
            ViewData["AcademicYear"] = AcademicYear;
            ViewData["ReportNumber"] = GenerateReportNumber();
            ViewData["Dean"] = dean;
            var ReportData = enrollmentRepository.GetEnrollmentReport(CourseCode, Semester, AcademicYear);
            return PartialView("_CourseEnroll", ReportData);
        }
        public static string GenerateReportNumber()
        {
            // Get the current date in YYYYMMDD format
            string formattedDate = DateTime.Now.ToString("yyyyMMdd");

            // Generate a random 6-digit number
            Random random = new Random();
            int randomNumber = random.Next(100000, 999999); // Ensures a 6-digit number

            // Combine "R", date, and random number to form the report number
            return $"R{formattedDate}{randomNumber}";
        }
    }
}
