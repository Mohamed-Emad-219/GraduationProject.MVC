using AspNetCoreGeneratedDocument;
using GP.BLL.Interfaces;
using GP.BLL.Repositories;
using GP.DAL.Context;
using GP.DAL.Models;
using GraduationProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        private readonly IFacultyMemberRepsitory facultyMemberRepsitory;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IPetitionRequestRepository petitionRequestRepository;
        private readonly IPetitionCourseRepository petitionCourseRepository;
        private readonly ICourseRepository courseRepository;
        private readonly IRevenuereport revenuereport;

        public FinancialAffairsController(IRevenuereport _revenuereport, ICourseRepository _courseRepository, IPetitionCourseRepository _petitionCourseRepository, IPetitionRequestRepository _petitionRequestRepository, IDepartmentRepository _departmentRepository, IFacultyMemberRepsitory _facultyMemberRepsitory, IFinancialAffairsRepository _financialAffairsRepository, UserManager<GPUser> _userManager, IReceiptRepository _receiptRepository, ICollegeRepository _collegeRepository, IEnrollmentRepository _enrollmentRepository, ITermRepository _termRepository, ITermCourseRepository _termCourseRepository, IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
            termRepository = _termRepository;
            termCourseRepository = _termCourseRepository;
            enrollmentRepository = _enrollmentRepository;
            collegeRepository = _collegeRepository;
            receiptRepository = _receiptRepository;
            userManager = _userManager;
            financialAffairsRepository = _financialAffairsRepository;
            facultyMemberRepsitory = _facultyMemberRepsitory;
            departmentRepository = _departmentRepository;
            petitionRequestRepository = _petitionRequestRepository;
            petitionCourseRepository = _petitionCourseRepository;
            courseRepository = _courseRepository;
            revenuereport = _revenuereport;
        }
        [Authorize(Roles = "FinancialAffairs")]
        public IActionResult PaymentDetails()
        {
            return View();
        }
        [Authorize(Roles = "FinancialAffairs")]
        public IActionResult Search(int id)
        {
            var student = _studentRepository.GetStudentById(id);
            if (student == null)
            {
                return NotFound("Student not found.");
            }

            var nextTerm = enrollmentRepository.GetNextTermForStudent(student.Id);
            if (nextTerm == null)
            {
                return NotFound("No suitable term found.");
            }

            var courses = termCourseRepository.GetCoursesPerTerm(nextTerm.Id, student.Level, student.DeptId);
            if (courses == null)
            {
                return NotFound("No suitable courses found.");
            }
            int totalPrice = 0;
            foreach (var course in courses)
            {
                totalPrice += termCourseRepository.GetCoursePrice(course.Code, nextTerm.Id);
            }

            ViewData["Student"] = student;
            ViewData["Term"] = nextTerm;
            ViewData["Courses"] = courses;
            ViewData["TotalPrice"] = totalPrice;

            return PartialView("_StudentDetails");
        }
        [Authorize(Roles = "FinancialAffairs")]
        public async Task<IActionResult> Receipt(int StudentId, string StudentName, 
            int Level, int RegisteredYear, string Semester, int AcademicYear, int Amount)
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
                enrollmentRepository.EnrollStudentToNextTerm(Receipt.StudentId);
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
            ViewData["Deans"] = facultyMemberRepsitory.GetDeans();
            ViewData["Colleges"] = collegeRepository.GetColleges();
            ViewData["Department"] = departmentRepository.GetDepartments();
            return View();
        }
        [Route("/FinancialAffairs/AddPetitionRequest")]
        [Authorize(Roles = "FinancialAffairs")]
        [HttpPost]
        public IActionResult AddPetitionRequest(PetitionRequestVM model) {
            // If validation fails, return the form with errors
            if (!ModelState.IsValid) return View(model); 
            // Create a new PetitionRequest entity (assuming a PetitionRequest table exists)
            var petition = new PetitionRequest
            {
                DeanId = model.DeanId,
                CollegeId = model.CollegeId,
                DeptId = model.DeptId,
                StudentName = model.StudentName,
                RegistrationNumber = model.RegistrationNumber,
                Semester = model.Semester,
                Date = model.Date,
                NumberOfCourses = model.NumberOfCourses,
                AmountPaid = model.AmountPaid,
                PaymentDate = model.PaymentDate
            };
            petitionRequestRepository.AddPetition(petition);
            // Save Courses in a separate table 
            foreach (var courseName in model.Courses) {
                var course = new PetitionCourse
                {
                    PetitionRequestId = petition.Id, // Link to petition
                    CourseCode = courseRepository.GetCourseCodeByName(courseName)
                };
                petitionCourseRepository.AddPetitionCourse(course);
            }
            return RedirectToAction("Index", "Home"); // Redirect to home page
        }
        [Authorize(Roles="FinancialAffairs")]
        public IActionResult RevenueReport()
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
        public async Task<IActionResult> Revenue(DateTime Date)
        {
            var user = await userManager.GetUserAsync(User);
            ViewData["user"] = financialAffairsRepository.GetFinancialAffairsByUserId(user.Id);
            ViewData["date"] = Date;
            ViewData["ReportNumber"] = GenerateReportNumber();
            ViewData["reportDate"] = DateTime.Now.ToString("dd-MM-yyyy"); 
            var receipts = revenuereport.GetAllReceipts(Date);
            return PartialView("_Revenue", receipts);
        }
    }
}
