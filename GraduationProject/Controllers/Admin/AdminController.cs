using GP.BLL.Interfaces;
using GP.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Controllers.Admin
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ICollegeRepository _collegeRepository;
        private readonly IFacultyMemberRepsitory _facultyMemberRepsitory;
        public AdminController(IFacultyMemberRepsitory facultyMemberRepsitory, IDepartmentRepository departmentRepository, ICourseRepository courseRepository, ICollegeRepository collegeRepository)
        {
            _departmentRepository = departmentRepository;
            _courseRepository = courseRepository;
            _collegeRepository = collegeRepository;
            _facultyMemberRepsitory = facultyMemberRepsitory;
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
        [HttpPost]
        public IActionResult CourseEdit(GP.DAL.Models.Course course)
        {
            _courseRepository.UpdateCourse(course);
            return RedirectToAction("Dashboard");
        }
        public IActionResult DepartmentEditPage(int id)
        {
            ViewData["Department"] = _departmentRepository.GetDepartmentById(id);
            ViewData["Colleges"] = _collegeRepository.GetColleges();
            ViewData["Heads"] = _facultyMemberRepsitory.GetHeads();
            return View();
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
    }
}
