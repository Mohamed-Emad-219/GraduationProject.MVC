using GP.BLL.Interfaces;
using GP.DAL.Context;
using GP.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Controllers.Course
{
    public class CourseController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IDepartmentRepository _departmentRepository;


        public CourseController(ICourseRepository courseRepository, IDepartmentRepository departmentRepository)
        {
            _courseRepository = courseRepository;
            _departmentRepository = departmentRepository;
        }

        public IActionResult Add(GP.DAL.Models.Course course)
        {
            if (!ModelState.IsValid)
            {
                // If validation fails, reload the form with the department data
                ViewData["Departments"] = _departmentRepository.GetDepartments();
                return RedirectToAction("CourseAddPage", "Admin", course.Code);
            }

            _courseRepository.AddCourse(course);

            return RedirectToAction("Dashboard", "Admin");
        }
        public IActionResult Delete(string code)
        {
            _courseRepository.DeleteCourseAsync(code);
            return RedirectToAction("Dashboard", "Admin");
        }

        [HttpGet]
        public JsonResult GetCourses()
        {
            var courses = _courseRepository.GetAllCourses();
            return Json(courses);
        }
    }
}
