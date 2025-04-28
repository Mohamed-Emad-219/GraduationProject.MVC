using Microsoft.AspNetCore.Mvc;
using GP.DAL.Models;
using GP.DAL.Context;
using Microsoft.AspNetCore.Authorization;
using GP.BLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace GraduationProject.Controllers.StudentAffairs
{
    
    public class StudentAffairsController : Controller
    {
        private readonly ICollegeRepository _collegeRepository;
        private readonly IApplicationRepository applicationRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IReceiptRepository receiptRepository;
        private readonly UserManager<GPUser> userManager;
        private readonly IStudentAffairsRepository studentAffairsRepository;
        private readonly IDepartmentRepository departmentRepository;
        public StudentAffairsController(IDepartmentRepository _departmentRepository, IStudentAffairsRepository _studentAffairsRepository, UserManager<GPUser> _userManager, IReceiptRepository _receiptRepository, IStudentRepository _studentRepository, IApplicationRepository _applicationRepository, ICollegeRepository collegeRepository)
        {
            _collegeRepository = collegeRepository;
            applicationRepository = _applicationRepository;
            studentRepository = _studentRepository;
            receiptRepository = _receiptRepository;
            userManager = _userManager;
            studentAffairsRepository = _studentAffairsRepository;
            departmentRepository = _departmentRepository;
        }

        [HttpGet]
        public IActionResult NewApplication()
        {
            ViewData["Colleges"] = _collegeRepository.GetColleges();
            return View("NewApplication");
        }
        [HttpPost]
        [Route("StudentAffairs/NewApplicationAdd")]
        public IActionResult NewApplicationAdd(GP.DAL.Models.Student student)
        {
            if (ModelState.IsValid)
            {
                studentRepository.AddStudent(student);

                var application = new Application
                {
                    Status = Status.Pending,
                    CreatedAt = DateTime.Now,
                    StudentId = student.Id
                };

                applicationRepository.AddApplication(application);
                studentRepository.AddApplicationIdToStudent(student.Id, application.Id);
                return RedirectToAction("Index", "Home");
            }
            ViewData["Colleges"] = _collegeRepository.GetColleges();
            return View("NewApplication", student); // Return the form with validation errors if any
        }
        [Authorize(Roles = "StudentAffairs")]
        public IActionResult ApplicationTable()
        {
            ViewData["Applications"] = applicationRepository.GetApplications();
            return View();
        }
        [Authorize(Roles = "StudentAffairs")]
        public IActionResult ApplicationInfo(int Id)
        {
            ViewData["App"] = applicationRepository.GetApplicationById(Id);
            ViewData["Student"] = studentRepository.GetStudentByApplicationId(Id);
            return View();
        }
        [Authorize(Roles = "StudentAffairs")]
        public async Task<IActionResult> ApproveApplication(int Id)
        {
            var application = applicationRepository.GetApplicationById(Id);
            var student = studentRepository.GetStudentByApplicationId(Id);
            if (application != null)
            {
                application.Status = Status.Approved;
                var empauth = await userManager.GetUserAsync(User);
                var emp = studentAffairsRepository.GetStudentAffairsByUserId(empauth.Id);
                application.StudentAffairsId = emp.Id;
                var email = $"{student.FirstName.ToLower()}.{student.LastName.ToLower()}@g.com";
                var user = new GPUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };
                
                var result = await userManager.CreateAsync(user, "qweQWE123!!");

                if (result.Succeeded)
                {
                    student.UserId = user.Id;
                    await userManager.AddToRoleAsync(user, "Student");
                }
                student.Level = 1;
                student.Group = "G1";
                var general = departmentRepository.GetDepartmentByName("General");
                student.DeptId = general.Id;
                studentRepository.UpdateStudent(student);
                applicationRepository.UpdateApplication(application);
            }
            return RedirectToAction("ApplicationTable");
        }
        [Authorize(Roles = "StudentAffairs")]
        public IActionResult RejectApplication(int Id)
        {
            var application = applicationRepository.GetApplicationById(Id);
            if (application != null)
            {
                application.Status = Status.Rejected;
                applicationRepository.UpdateApplication(application);
            }
            return RedirectToAction("ApplicationTable");
        }
        [Authorize(Roles = "StudentAffairs")]
        public IActionResult ReceiptTable()
        {
            ViewData["Receipts"] = receiptRepository.GetReceipts();
            return View();
        }
        [Authorize(Roles = "StudentAffairs")]
        public async  Task<IActionResult> ReceiptInfo(int Id)
        {
            ViewData["ReceiptStudent"] = receiptRepository.GetReceiptWithStudentById(Id);
            ViewData["ReceiptFinancial"] = receiptRepository.GetReceiptWithFinancialById(Id);

            var user = await userManager.GetUserAsync(User);
            ViewData["User"] = studentAffairsRepository.GetStudentAffairsByUserId(user.Id);
            return View();
        }
        [Authorize(Roles = "StudentAffairs")]
        public IActionResult UpdateReceipt(int Id, int StudentAffairsId)
        {
            receiptRepository.UpdateReceiptStudentAffairId(Id, StudentAffairsId);
            return RedirectToAction("ReceiptTable");
        }
        [Authorize("ManagerOfStudentAffairs")]
        public IActionResult ManagerStats()
        {
            return View();
        }
    }
}
