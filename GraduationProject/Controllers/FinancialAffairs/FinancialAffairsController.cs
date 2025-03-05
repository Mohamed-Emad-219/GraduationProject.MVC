using AspNetCoreGeneratedDocument;
using GP.BLL.Interfaces;
using GP.BLL.Repositories;
using GP.DAL.Context;
using GP.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly IReceiptRepository receiptRepository;
        private readonly UserManager<GPUser> userManager;
        private readonly IFinancialAffairsRepository financialAffairsRepository;
        public FinancialAffairsController(IFinancialAffairsRepository _financialAffairsRepository, UserManager<GPUser> _userManager, IReceiptRepository _receiptRepository, ICollegeRepository _collegeRepository, IEnrollmentRepository _enrollmentRepository, ITermRepository _termRepository, ITermCourseRepository _termCourseRepository, IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
            termRepository = _termRepository;
            termCourseRepository = _termCourseRepository;
            enrollmentRepository = _enrollmentRepository;
            collegeRepository = _collegeRepository;
            receiptRepository = _receiptRepository;
            userManager = _userManager;
            financialAffairsRepository = _financialAffairsRepository;
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
            Term nextTerm;
            // Retrieve the last term the student was enrolled in
            var lastTerm = enrollmentRepository.GetLastTermForStudent(student.Id);
            if (lastTerm == null)
            {
                // NEW STUDENT: Set default term as Level 1, Fall, Current Year
                int currentYear = DateTime.Now.Year;
                nextTerm = termRepository.GetTermByDetails(1, SemesterType.Fall, currentYear);

                if (nextTerm == null)
                {
                    return NotFound("No term found for Level 1 Fall Semester.");
                }
            }
            else
            {
                // EXISTING STUDENT: Determine the next semester
                SemesterType nextSemester = (lastTerm.Semester == SemesterType.Spring) ? SemesterType.Fall : SemesterType.Spring;
                int nextAcademicYear = (nextSemester == SemesterType.Spring) ? lastTerm.AcademicYear + 1 : lastTerm.AcademicYear;

                // Get next term details
                nextTerm = termRepository.GetTermByDetails(lastTerm.Level, nextSemester, nextAcademicYear);
                if (nextTerm == null)
                {
                    return NotFound("No next term found.");
                }
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
        public async Task<IActionResult> Receipt(int StudentId, string StudentName, int Level, int RegisteredYear, string Semester, int AcademicYear, int Amount)
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
            var user = await userManager.GetUserAsync(User);
            var emp = financialAffairsRepository.GetFinancialAffairsByUserId(user.Id);
            ViewData["User"] = emp;
            return View();
        }
        [Authorize(Roles = "FinancialAffairs")]
        [Route("/FinancialAffairs/AddReceipt")]
        public async Task<IActionResult> AddReceipt(Receipt Receipt)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                var emp = financialAffairsRepository.GetFinancialAffairsByUserId(user.Id);
                Receipt.FinancialAffairsId = emp.Id;
                receiptRepository.AddReceipt(Receipt);
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Receipt");
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
        [Authorize(Roles = "FinancialAffairs")]
        public IActionResult AddPetitonRequest()
        {
            return RedirectToAction("Index", "Home");
        }

    }
}
