using Microsoft.AspNetCore.Mvc;
using GP.DAL.Context;
using Microsoft.AspNetCore.Authorization;
using GP.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GraduationProject.Controllers.Advisor
{
    [Authorize(Roles = "Advisor")]

    public class AdvisorController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ICollegeRepository _collegeRepository;
        private readonly IFacultyMemberRepsitory _facultyMemberRepsitory;
        private readonly IPlaceRepository _placeRepository;
        public AdvisorController(IPlaceRepository placeRepository, IFacultyMemberRepsitory facultyMemberRepsitory, IDepartmentRepository departmentRepository, ICourseRepository courseRepository, ICollegeRepository collegeRepository)
        {
            _departmentRepository = departmentRepository;
            _courseRepository = courseRepository;
            _collegeRepository = collegeRepository;
            _facultyMemberRepsitory = facultyMemberRepsitory;
            _placeRepository = placeRepository;

        }
        public IActionResult Dashboard()
        {
            ViewData["Courses"] = _courseRepository.GetCourses();
            ViewData["Departments"] = _departmentRepository.GetDepartments();
            return View();
        }
        public IActionResult SemesterInfoPage()
        {
            return View();
        }
        public IActionResult InstructorInfoPage()
        {
            ViewData["Instructors"] = _facultyMemberRepsitory.GetAll();
            ViewData["Courses"] = _courseRepository.GetCourses();
            ViewData["Places"] = _placeRepository.GetAll();
            ViewData["Coursess"] = _courseRepository.GetCoursesNameCode();
            ViewData["Placess"] = _placeRepository.GetPlacesNameId();
            return View();
        }
        public IActionResult DefaultSchedule(int List, string Semester, int Level, List<string> Course)
        {
            ViewData["List"] = List;
            ViewData["Semester"] = Semester;
            ViewData["Level"] = Level;
            ViewData["Course"] = Course;
            return View();
        }
        public IActionResult DefaultScheduleEntry() {
            return View("Dashboard");
        }
        [HttpGet]
        public IActionResult RequestCourseModification() {
            return View();
        }
        [HttpPost]
        public IActionResult RequestCourseModification(string Code, string Changes)
        {

            return View();
        }
        [HttpGet]
        public IActionResult RequestDepartmentModification() {

            return View();
        }
        [HttpPost]
        public IActionResult RequestDepartmentModification(int Code, string Changes)
        {
            return View();
        }
        public IActionResult Requests()
        {
            return View();
        }
        public IActionResult PetitionRequestDetails()
        {
            return View();
        }
        public IActionResult ResultPetition()
        {
            return View();
        }
        public IActionResult AddResultPetition()
        {
            return View();
        }
    }
}
