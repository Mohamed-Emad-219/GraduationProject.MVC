using AspNetCoreGeneratedDocument;
using GP.BLL.Interfaces;
using GP.BLL.Repositories;
using GP.DAL.Context;
using GP.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Controllers.FinancialAffairs
{ 
    public class FinancialAffairsController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ITermCourseRepository termCourseRepository;
        private readonly ITermRepository termRepository;
        private readonly IEnrollmentRepository enrollmentRepository;
        private readonly ICollegeRepository collegeRepository;
        public FinancialAffairsController(ICollegeRepository _collegeRepository, IEnrollmentRepository _enrollmentRepository, ITermRepository _termRepository, ITermCourseRepository _termCourseRepository, IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
            termRepository = _termRepository;
            termCourseRepository = _termCourseRepository;
            enrollmentRepository = _enrollmentRepository;
            collegeRepository = _collegeRepository;
        }
        [Authorize(Roles = "FinancialAffairs")]
        public IActionResult PaymentDetails()
        {
            return View();
        }
        [Authorize(Roles = "FinancialAffairs")]
        public IActionResult Search(int id)
        {
            var Id = id;

            var student = _studentRepository.GetStudentById(Id);
            if (student == null)
            {
                return NotFound("Student not found.");
            }
            // Retrieve the last term the student was enrolled in
            var lastTerm = enrollmentRepository.GetLastTermForStudent(student.Id);
            if (lastTerm == null)
            {
                return NotFound("No enrollment records found for this student.");
            }

            // Determine next semester & academic year based on last term
            SemesterType nextSemester = (lastTerm.Semester == SemesterType.Spring) ? SemesterType.Fall : lastTerm.Semester = SemesterType.Spring;
            int nextAcademicYear = (nextSemester == SemesterType.Spring) ? lastTerm.AcademicYear + 1 : lastTerm.AcademicYear;

            // Get next term details
            var nextTerm = termRepository.GetTermByDetails(lastTerm.Level, nextSemester, nextAcademicYear);
            if (nextTerm == null)
            {
                return NotFound("No next term found.");
            }

            // Retrieve courses for the next term
            var courses = termCourseRepository.GetCoursesPerTerm(nextTerm.Id);

            // Calculate total price
            int totalPrice = 0;
            foreach(var course in courses)
            {
                totalPrice += termCourseRepository.GetCoursePrice(course.Code, nextTerm.Id);
            }

            // Pass data to the view
            ViewData["Student"] = student;
            ViewData["Term"] = nextTerm;
            ViewData["Courses"] = courses;
            ViewData["TotalPrice"] = totalPrice;

            return PartialView("_StudentDetails");
            
        }
        [Authorize(Roles = "FinancialAffairs")]
        public IActionResult Receipt(int StudentId, string StudentName, int Level, int RegisteredYear, string Semester, int AcademicYear, int Amount)
        {
            ViewData["CollegeName"] = collegeRepository.GetCollageNameByStudentId(StudentId);
            ViewData["CollegeID"] = collegeRepository.GetCollageIdByStudentId(StudentId);
            ViewData["Name"] = StudentName;
            ViewData["Level"] = Level;
            ViewData["Reg"] = RegisteredYear;
            ViewData["Semester"] = Semester;
            ViewData["StudentId"] = StudentId;
            ViewData["AcademicYear"] = AcademicYear;
            ViewData["Amount"] = Amount;
            return View();
        }
        [Authorize(Roles = "FinancialAffairs")]
        public IActionResult AddReceipt(Receipt Receipt)
        {

            return View("PaymentDetails");
        }
        [Authorize(Roles = "FinancialAffairs")]
        public IActionResult UpdateReceipt()
        {
            return RedirectToAction("ReceiptTable", "StudentAffairs");
        }
        [Authorize(Roles = "ManagerOfFinancialAffairs")]
        public IActionResult ManagerStats()
        {
            return View();
        }
        [Authorize(Roles = "FinancialAffairs")]
        public IActionResult PetitonRequest()
        {
            return View();
        }

    }
}
