using GP.BLL.Interfaces;
using GP.BLL.Repositories;
using GP.DAL.Models;
using GraduationProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Controllers.Admin
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ICollegeRepository _collegeRepository;
        private readonly IFacultyMemberRepsitory _facultyMemberRepsitory;
        private readonly ITermRepository termRepository;
        private readonly ITermCourseRepository termCourseRepository;
        private readonly IEnrollmentRepository enrollmentRepository;
        private readonly UserManager<GPUser> _userManager;
        private readonly IAdminRepository adminRepository;
        public AdminController(IAdminRepository _adminRepository, UserManager<GPUser> userManager, IEnrollmentRepository _enrollmentRepository, ITermCourseRepository _termCourseRepository, ITermRepository _termRepository, IFacultyMemberRepsitory facultyMemberRepsitory, IDepartmentRepository departmentRepository, ICourseRepository courseRepository, ICollegeRepository collegeRepository)
        {
            _departmentRepository = departmentRepository;
            _courseRepository = courseRepository;
            _collegeRepository = collegeRepository;
            _facultyMemberRepsitory = facultyMemberRepsitory;
            termRepository = _termRepository;
            termCourseRepository = _termCourseRepository;
            enrollmentRepository = _enrollmentRepository;
            _userManager = userManager;
            adminRepository = _adminRepository;
        }
        [HttpGet]
        public IActionResult AddTerm()
        {
            return View(new AddTermViewModel());
        }
        
        [HttpPost]
        [Route("Admin/TermAdd")]
        public IActionResult TermAdd(AddTermViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var term = new Term
            {
                AcademicYear = model.AcademicYear,
                Level = model.Level,
                Semester = model.Semester,
                List = model.List
            };

            termRepository.AddTerm(term);
            TempData["SuccessMessage"] = "Term Added Successfully!";
            return RedirectToAction("AddCoursesTerm"); // or wherever you want
        }
        [HttpGet]
        public IActionResult AddCoursesTerm()
        {
            var viewModel = new BulkCoursesTermFormVM
            {
                AvailableTerms = termRepository.GetTerms(),
                AvailableCourses = _courseRepository.GetCourses()
            };

            return View(viewModel);
        }
        [HttpPost]
        public IActionResult BulkAddCoursesTerm(BulkCoursesTermFormVM model)
        {
            if (ModelState.IsValid)
            {
                foreach (var course in model.Courses)
                {
                    var coursesTerm = new CoursesTerm
                    {
                        TermId = model.TermId,
                        CourseCode = course.CourseCode,
                        Price = course.Price
                    };
                    termCourseRepository.AddCourseTerm(coursesTerm);
                    //_context.CoursesTerms.Add(coursesTerm);
                }
                TempData["SuccessMessage"] = "Courses Added Successfully!";
                return RedirectToAction("Dashboard"); // Redirect where you like
            }

            model.AvailableTerms = termRepository.GetTerms();
                //AvailableCourses = _courseRepository.GetCourses()
            model.AvailableCourses = _courseRepository.GetCourses();
            return View(model);
        }
        public IActionResult Dashboard()
        {
            ViewData["Courses"] = _courseRepository.GetCourses();
            ViewData["Departments"] = _departmentRepository.GetDepartments();
            return View();
        }
        public IActionResult CourseAddPage()
        {
            ViewData["Departments"] = _departmentRepository.GetDepartments();
            return View();
        }
        public IActionResult DepartmentAddPage()
        {
            ViewData["Colleges"] = _collegeRepository.GetColleges();
            ViewData["Heads"] = _facultyMemberRepsitory.GetHeads();
            return View();
        }
        public async Task<IActionResult> CourseEditPage(string id)
        {
            ViewData["Course"] = await _courseRepository.GetCourseById(id);
            ViewData["Departments"] = _departmentRepository.GetDepartments();
            return View();
        }
        public IActionResult DepartmentEditPage(int id)
        {
            ViewData["Department"] = _departmentRepository.GetDepartmentById(id);
            ViewData["Colleges"] = _collegeRepository.GetColleges();
            ViewData["Heads"] = _facultyMemberRepsitory.GetHeads();
            return View();
        }

        [HttpPost]
        public IActionResult CourseEdit(GP.DAL.Models.Course course)
        {
            _courseRepository.UpdateCourse(course);
            TempData["SuccessMessage"] = "Course Updated Successfully!";
            return RedirectToAction("Dashboard");
        }
        
        [HttpPost]
        public IActionResult DepartmentEdit(GP.DAL.Models.Department dep)
        {
            _departmentRepository.UpdateDepartment(dep);
            TempData["SuccessMessage"] = "Department Updated Successfully!";
            return RedirectToAction("Dashboard");
        }
        public IActionResult ShowRequests()
        {
            return View();
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
        public IActionResult CoursesReport()
        {
            return View();
        }
        public async Task<IActionResult> CoursesReportPartial(int year, SemesterType semester)
        {
            ViewData["Semester"] = semester;
            ViewData["AcademicYear"] = year;
            ViewData["ReportNumber"] = GenerateReportNumber();
            var user = await _userManager.GetUserAsync(User);
            var advisor = adminRepository.GetAdminByUserId(user.Id);
            ViewData["user"] = advisor;
            ViewData["Date"] = DateTime.Now.ToString("dd-MM-yyyy");
            var courses = enrollmentRepository.GetCoursesReport(year, semester);
            return PartialView("_CoursesReport", courses);
        }
        public IActionResult DepartmentsReport()
        {
            return View();
        }
        public async Task<IActionResult> DepartmentsReportPartial(int year, SemesterType semester)
        {
            ViewData["Semester"] = semester;
            ViewData["AcademicYear"] = year;
            ViewData["ReportNumber"] = GenerateReportNumber();
            var user = await _userManager.GetUserAsync(User);
            var advisor = adminRepository.GetAdminByUserId(user.Id);
            ViewData["user"] = advisor;
            ViewData["Date"] = DateTime.Now.ToString("dd-MM-yyyy");
            var deps = enrollmentRepository.GetDepartmentsReport(year, semester);
            return PartialView("_DepartmentsReport", deps);
        }
    }
}
