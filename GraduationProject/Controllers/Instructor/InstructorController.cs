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
        private readonly ITeachingHoursReport teachingHoursReport;
        public InstructorController(ITeachingHoursReport _teachingHoursReport, IEnrollmentRepository _enrollmentRepository, ICollegeRepository collegeRepository, IDepartmentRepository departmentRepository, IFacultyMemberRepsitory facultyMemberRepsitory, UserManager<GPUser> userManager, IInstructorScheduleRepositroy instructorScheduleRepositroy)
        {
            //_context = context; // Dependency Injection
            _instructorScheduleRepositroy = instructorScheduleRepositroy;
            _userManager = userManager;
            _facultyMemberRepsitory = facultyMemberRepsitory;
            _departmentRepository = departmentRepository;
            _collegeRepository = collegeRepository;
            enrollmentRepository = _enrollmentRepository;
            teachingHoursReport = _teachingHoursReport;
        }

        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> InstructorSchedule()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;
            var inst = _facultyMemberRepsitory.GetFacultyByUserIdAsync(userId);
            ViewData["Schedule"] = _instructorScheduleRepositroy
                .GetInstructorScheduleByInstructorId(inst.Result.TeacherId);
            
            return View();
        }

        [Authorize(Roles = "Assistant")]
        public async Task<IActionResult> AssistantSchedule()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;
            var assistant = _facultyMemberRepsitory.GetFacultyByUserIdAsync(userId);
            ViewData["Schedule"] = _instructorScheduleRepositroy
                .GetAssistantScheduleByAssistantId(assistant.Result.TeacherId);
            return View();
        }
        [Authorize(Roles = "Dean")]
        public IActionResult CourseEnrollmentReport()
        {
            return View();
        }
        public async Task<IActionResult> SearchEnroll(string CourseCode, SemesterType Semester, int AcademicYear)
        {
            var user = await _userManager.GetUserAsync(User);
            var dean = await _facultyMemberRepsitory.GetFacultyByUserIdAsync(user.Id);
            await Task.Delay(1000);
            ViewData["Semester"] = Semester.ToString();
            ViewData["AcademicYear"] = AcademicYear;
            ViewData["ReportNumber"] = GenerateReportNumber();
            ViewData["Dean"] = dean;
            ViewData["Date"] = DateTime.Now.ToString("dd-MM-yyyy");
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
        [Authorize(Roles="Dean")]
        public IActionResult ShowSchedulesDean()
        {
            ViewData["inst"] = _facultyMemberRepsitory.GetInstructorsAssistants();
            return View();
        }
        [Authorize(Roles = "Head")]
        public async Task<IActionResult> ShowSchedulesHead()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;
            var head = await _facultyMemberRepsitory.GetFacultyByUserIdAsync(userId);
            ViewData["inst"] = _facultyMemberRepsitory.GetInstructorsAssistantsByDep(head.DeptId);
            return View();
        }
        public IActionResult SchedulesDean(string TeacherId)
        {
            ViewData["Schedule"] = _instructorScheduleRepositroy
                .GetSchedule(TeacherId);
            return PartialView("_ScheduleView");
        }
        [Authorize(Roles = "Dean")]
        public IActionResult TeachingHoursDean()
        {
            return View();
        }
        [Authorize(Roles = "Head")]
        public IActionResult TeachingHoursHead()
        {
            return View();
        }
        public async Task<IActionResult> TeachingHourHead(SemesterType Semester, int AcademicYear)
        {
            if (AcademicYear == 0)
            {
                return BadRequest("Enter Year.");
            }
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;
            var head = await _facultyMemberRepsitory.GetFacultyByUserIdAsync(userId);
            ViewData["Faculty"] = head;
            ViewData["insthead"] = teachingHoursReport.GetFacultyMembersHead(head.DeptId);
            ViewData["year"] = AcademicYear;
            ViewData["semester"] = Semester;
            ViewData["Date"] = DateTime.Now.ToString("dd-MM-yyyy");
            ViewData["number"] = GenerateReportNumber();
            var courseshead = teachingHoursReport.GetCoursesHead(head.DeptId ,Semester, AcademicYear);
            ViewData["totalfaculty"] = teachingHoursReport.TotalFacultyMembers();
            ViewData["totalhours"] = teachingHoursReport.TotalTeachingHoursAssigned();
            ViewData["overloadhours"] = teachingHoursReport.OverloadTeachingHours();

            return PartialView("_TeachingHour", courseshead);
        }
        public async Task<IActionResult> TeachingHourDean(SemesterType Semester, int AcademicYear)
        {
            if (AcademicYear == 0)
            {
                return BadRequest("Enter Year.");
            }
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;
            ViewData["Faculty"] = await _facultyMemberRepsitory.GetFacultyByUserIdAsync(userId);
            ViewData["instdean"] = teachingHoursReport.GetFacultyMembersDean();
            ViewData["year"] = AcademicYear;
            ViewData["semester"] = Semester;
            ViewData["Date"] = DateTime.Now.ToString("dd-MM-yyyy");
            ViewData["number"] = GenerateReportNumber();
            var coursesdean = teachingHoursReport.GetCoursesDean(Semester, AcademicYear);
            ViewData["totalfaculty"] = teachingHoursReport.TotalFacultyMembers();
            ViewData["totalhours"] = teachingHoursReport.TotalTeachingHoursAssigned();
            ViewData["overloadhours"] = teachingHoursReport.OverloadTeachingHours();

            return PartialView("_TeachingHour", coursesdean);
        }
    }
}
