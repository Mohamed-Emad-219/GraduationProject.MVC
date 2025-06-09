using Microsoft.AspNetCore.Mvc;
using GP.DAL.Context;
using Microsoft.AspNetCore.Authorization;
using GP.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using GP.DAL.Models;
using Microsoft.EntityFrameworkCore;
using GP.BLL.Repositories;
using GraduationProject.ViewModels;

namespace GraduationProject.Controllers.Advisor
{
    [Authorize(Roles = "Advisor")]

    public class AdvisorController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IFacultyMemberRepsitory _facultyMemberRepsitory;
        private readonly IPlaceRepository _placeRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IEnrollmentRepository enrollmentRepository;
        private readonly UserManager<GPUser> userManager;
        private readonly IAdvisorRepository advisorRepository;
        private readonly IPetitionRequestRepository petitionRequestRepository;
        private readonly IPetitionCourseRepository petitionCourseRepository;
        private readonly IStudentDistribution studentDistributionRepositroy;
        private readonly IResultPetitionRepository resultPetitionRepository;
        
        public AdvisorController(IStudentDistribution _studentDistribution, IResultPetitionRepository _resultPetitionRepository, IPetitionCourseRepository _petitionCourseRepository, IPetitionRequestRepository _petitionRequestRepository, IAdvisorRepository _advisorRepository, UserManager<GPUser> _userManager, IEnrollmentRepository _enrollmentRepository, IStudentRepository studentRepository, IPlaceRepository placeRepository, IFacultyMemberRepsitory facultyMemberRepsitory, IDepartmentRepository departmentRepository, ICourseRepository courseRepository)
        {
            _departmentRepository = departmentRepository;
            _courseRepository = courseRepository;
            _facultyMemberRepsitory = facultyMemberRepsitory;
            _placeRepository = placeRepository;
            _studentRepository = studentRepository;
            enrollmentRepository = _enrollmentRepository;
            userManager = _userManager;
            advisorRepository = _advisorRepository;
            petitionRequestRepository = _petitionRequestRepository;
            petitionCourseRepository = _petitionCourseRepository;
            studentDistributionRepositroy = _studentDistribution;
            resultPetitionRepository = _resultPetitionRepository;
            
        }
        public IActionResult Dashboard()
        {
            ViewData["Courses"] = _courseRepository.GetCourses();
            ViewData["Departments"] = _departmentRepository.GetDepartments();
            return View();
        }
        public IActionResult SemesterInfoPage()
        {
            var month = DateTime.Now.Month;
            string semester;
            ViewData["year"] = DateTime.Now.Year;

            if (month >= 9 || month <= 1)
            {
                semester = "Fall";
            }
            else if (month >= 2 && month <= 6)
            {
                semester = "Spring";
            }
            else
            {
                semester = "Summer";
            }
            ViewData["semester"] = semester;
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
            ViewData["PetitionRequests"] = petitionRequestRepository.GetPetitionRequests();
            return View();
        }
        public IActionResult PetitionRequestDetails(int Id)
        {
            ViewData["PetitionRequest"] = petitionRequestRepository.GetPetitionRequestById(Id);
            ViewData["PetitionCourses"] = petitionCourseRepository.GetPetitionCoursesByPetitionId(Id);
            return View();
        }
        public async Task<IActionResult> ResultPetition(int Id)
        {
            ViewData["PetitionRequest"] = petitionRequestRepository.GetPetitionRequestById(Id);
            ViewData["PetitionCourses"] = petitionCourseRepository.GetPetitionCoursesByPetitionId(Id);
            var user = await userManager.GetUserAsync(User);
            ViewData["User"] = advisorRepository.GetAdvisorByUserId(user.Id);
            return View();
        }
        [HttpPost]
        public IActionResult AddResultPetition(ResultPetition model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, 
                message = "Invalid data. Please check your inputs and try again." });
            var result = new ResultPetition {
                PetitionCourseId = model.PetitionCourseId,
                ErrorInCorrection = model.ErrorInCorrection,
                ApplicationSubmitted = model.ApplicationSubmitted,
                YearWorkAssessment = model.YearWorkAssessment,
                PracticalExamAssessment = model.PracticalExamAssessment,
                FinalExamAssessment = model.FinalExamAssessment,
                TotalGrade = model.TotalGrade,
                FinalGrade = model.FinalGrade,
                Notes = model.Notes,
                AdvisorId = model.AdvisorId
            };
            resultPetitionRepository.AddResultPetition(result);
            return Json(new { success = true, message = "Result added successfully!" });
        }

        public IActionResult IndividualStudentProgress()
        {
            return View();
        }
        public async Task<IActionResult> Search(string id)
        {
            var Id = id;

            var student = _studentRepository.GetStudentById(Id);
            if (student == null)
            {
                return NotFound("Student not found.");
            }
            
            // Pass data to the view
            ViewData["Student"] = student;
            ViewData["ReportNumber"] = GenerateReportNumber();
            int completedHours = enrollmentRepository.GetCompletedHoursForStudent(Id);
            ViewData["completedHours"] = completedHours;
            ViewData["remainingHours"] = 144 - completedHours;
            ViewData["completedCourses"] = enrollmentRepository.GetCompletedCoursesForStudent(Id);
            ViewData["Date"] = DateTime.Now.ToString("dd-MM-yyyy");
            var user = await userManager.GetUserAsync(User);
            var advisor = advisorRepository.GetAdvisorByUserId(user.Id);
            ViewData["Advisor"] = advisor;
            return PartialView("_StudentProgress");

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
        public IActionResult StudentDistributionReport()
        {
            return View();
        }
        public async Task<IActionResult> GetDistributionReport(int year,SemesterType semester)
        {
            if(year == 0)
            {
                return BadRequest("Enter Year.");
            }
            ViewData["year"] = year;
            ViewData["semester"] = semester;
            ViewData["totalStudents"] = await studentDistributionRepositroy.totalNumberofStudents(year, semester);
            ViewData["totalDepartments"] = await studentDistributionRepositroy.totalNumberofDepartment();
            ViewData["avgStudents"] = await studentDistributionRepositroy.AVGStudentperdepartment(year, semester);
            ViewData["HighestEnrollments"] = await studentDistributionRepositroy.HighestEnrollments(year, semester);
            ViewData["LowestEnrollments"] = await studentDistributionRepositroy.LowestEnrollments(year, semester);
            ViewData["Date"] = DateTime.Now.ToString("dd-MM-yyyy");
            ViewData["number"] = GenerateReportNumber();
            var departmentStats = await studentDistributionRepositroy.GetStudentsPerDepartment(year, semester);
            var user = await userManager.GetUserAsync(User);
            var advisor = advisorRepository.GetAdvisorByUserId(user.Id);
            ViewData["Advisor"] = advisor;
            ViewData["departmentStats"] = departmentStats;
            await Task.Delay(1000);
            return PartialView("_StudentDistribution");
        }
        public IActionResult StudentGrades()
        {
            return View();
        }
        public async Task<IActionResult> StudentGradesSearch(string courseCode, int year, SemesterType semester)
        {
            ViewData["Semester"] = semester;
            ViewData["AcademicYear"] = year;
            ViewData["ReportNumber"] = GenerateReportNumber();
            var user = await userManager.GetUserAsync(User);
            var advisor = advisorRepository.GetAdvisorByUserId(user.Id);
            ViewData["user"] = advisor;
            ViewData["Date"] = DateTime.Now.ToString("dd-MM-yyyy");
            var studentGrades = enrollmentRepository.GetStudentGrades(year, semester, courseCode);
            var summary = enrollmentRepository.GetSemesterEvaluationSummary(year, semester, courseCode);

            var viewModel = new GradesReportViewModel
            {
                Department = _departmentRepository.GetDepartmentNameByCourseCode(courseCode), // you can replace
                Course = _courseRepository.GetCourseNameByCode(courseCode),     // you can replace
                Instructor = _facultyMemberRepsitory.GetInstructorNameByCourseCode(courseCode), // you can replace
                StudentGrades = studentGrades,
                Summary = summary
            };
            return PartialView("_GradesReport", viewModel);
        }
    }
}
