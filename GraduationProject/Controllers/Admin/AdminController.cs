using GP.BLL.Interfaces;
using GP.DAL.Models;
using GraduationProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
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

        public AdminController(ITermCourseRepository _termCourseRepository, ITermRepository _termRepository, IFacultyMemberRepsitory facultyMemberRepsitory, IDepartmentRepository departmentRepository, ICourseRepository courseRepository, ICollegeRepository collegeRepository)
        {
            _departmentRepository = departmentRepository;
            _courseRepository = courseRepository;
            _collegeRepository = collegeRepository;
            _facultyMemberRepsitory = facultyMemberRepsitory;
            termRepository = _termRepository;
            termCourseRepository = _termCourseRepository;
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

            return RedirectToAction("AddCourseTerm"); // or wherever you want
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
        public IActionResult CourseEditPage(string code)
        {
            ViewData["Course"] = _courseRepository.GetCourseById(code);
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
            return RedirectToAction("Dashboard");
        }
        
        [HttpPost]
        public IActionResult DepartmentEdit(GP.DAL.Models.Department dep)
        {
            _departmentRepository.UpdateDepartment(dep);
            return RedirectToAction("Dashboard");
        }
        public IActionResult ShowRequests()
        {
            return View();
        }
        public IActionResult CoursesReport()
        {
            return View();
        }
        public IActionResult DepartmentsReport()
        {
            return View();
        }
    }
}
