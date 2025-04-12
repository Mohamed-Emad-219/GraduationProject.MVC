using GP.BLL.Interfaces;
using GP.DAL.Context;
using GP.DAL.Models;
using GraduationProject.Controllers.Home;
using GraduationProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace GraduationProject.Controllers.Auth
{
    public class AuthController : Controller
    {
        private readonly UserManager<GPUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<GPUser> _signInManager;
        private readonly IStudentRepository studentRepository;
        private readonly IFacultyMemberRepsitory facultyMemberRepsitory;
        private readonly IAdvisorRepository advisorRepository;
        private readonly IFinancialAffairsRepository financialAffairsRepository;
        private readonly IStudentAffairsRepository studentAffairsRepository;
        private readonly IAdminRepository adminRepository;
        private readonly IFollowUpRepository followUpRepository;
       public AuthController(IFollowUpRepository _followUpRepository, IAdminRepository _adminRepository, IStudentAffairsRepository _studentAffairsRepository, IFinancialAffairsRepository _financialAffairsRepository, IAdvisorRepository _advisorRepository, IFacultyMemberRepsitory _facultyMemberRepsitory, IStudentRepository _studentRepository, UserManager<GPUser> userManager,SignInManager<GPUser> signInManager, RoleManager<IdentityRole> roleManager
                                 )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            studentRepository = _studentRepository;
            facultyMemberRepsitory = _facultyMemberRepsitory;
            advisorRepository = _advisorRepository;
            financialAffairsRepository = _financialAffairsRepository;
            studentAffairsRepository = _studentAffairsRepository;
            adminRepository = _adminRepository;
            followUpRepository = _followUpRepository;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new GPUser
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //bool userRoleExists = await _roleManager.RoleExistsAsync("User");
                    //if (!userRoleExists)
                    //{
                    //    await _roleManager.CreateAsync(new IdentityRole("User"));
                    //}
                    //await _userManager.AddToRoleAsync(user, "User");
                    return RedirectToAction(nameof(Login));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, true);
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }

                        ModelState.AddModelError(string.Empty, "Login Failed");
                    }
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult ForgetPassword()
        {
            return View("ForgetPassword");
        }
        public IActionResult VerifyEmail()
        {
            return Redirect("ForgetPassword");
        }
        public IActionResult VerifyCode()
        {
            return View("NewPassword");
        }
        public IActionResult VerifyPassword()
        {
            return View("Login");
        }
        public async Task<IActionResult> ShowProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            var f = await facultyMemberRepsitory.GetFacultyByUserIdAsync(user.Id);
            var s = studentRepository.GetStudentByUserId(user.Id);
            var ff = financialAffairsRepository.GetFinancialAffairsByUserId(user.Id);
            var ss = studentAffairsRepository.GetStudentAffairsByUserId(user.Id);
            var adv = advisorRepository.GetAdvisorByUserId(user.Id);
            var adm = adminRepository.GetAdminByUserId(user.Id);
            var followup = followUpRepository.GetFollowUpByUserId(user.Id);
            ViewData["Email"] = user.Email;
            if (f != null) return View("Profile", f);
            else if(s != null) return View("Profile", s);
            else if(ff != null)  return View("Profile", ff);
            else if (ss != null) return View("Profile", ss);
            else if (adv != null) return View("Profile", adv);
            else if (adm != null) return View("Profile", adm);
            else if (followup != null) return View("Profile", followup);
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            var f = await facultyMemberRepsitory.GetFacultyByUserIdAsync(user.Id);
            var s = studentRepository.GetStudentByUserId(user.Id);
            var ff = financialAffairsRepository.GetFinancialAffairsByUserId(user.Id);
            var ss = studentAffairsRepository.GetStudentAffairsByUserId(user.Id);
            var adv = advisorRepository.GetAdvisorByUserId(user.Id);
            var adm = adminRepository.GetAdminByUserId(user.Id);
            var followup = followUpRepository.GetFollowUpByUserId(user.Id);
            ViewData["Email"] = user.Email;
            if (f != null) return View("EditProfile", f);
            else if (s != null) return View("EditProfile", s);
            else if (ff != null) return View("EditProfile", ff);
            else if (ss != null) return View("EditProfile", ss);
            else if (adv != null) return View("EditProfile", adv);
            else if (adm != null) return View("EditProfile", adm);
            else if (followup != null) return View("EditProfile", followup);
            return RedirectToAction("ShowProfile");
        }
        [HttpPost]
        [Route("/Auth/UpdateProfileInfo")]
        public async Task<IActionResult> UpdateProfileInfo(string Email, string Address, string MobilePhone)
        {
            var user = await _userManager.GetUserAsync(User);
            var f = await facultyMemberRepsitory.GetFacultyByUserIdAsync(user.Id);
            var s = studentRepository.GetStudentByUserId(user.Id);
            var ff = financialAffairsRepository.GetFinancialAffairsByUserId(user.Id);
            var ss = studentAffairsRepository.GetStudentAffairsByUserId(user.Id);
            var adv = advisorRepository.GetAdvisorByUserId(user.Id);
            var adm = adminRepository.GetAdminByUserId(user.Id);
            var followup = followUpRepository.GetFollowUpByUserId(user.Id);
            ViewData["Email"] = user.Email;
            if (f != null) await facultyMemberRepsitory.UpdateFacultyAsync(f.Id, Email, Address, MobilePhone);
            else if (s != null) await studentRepository.UpdateStudentAsync(s.Id, Email, Address, MobilePhone);
            else if (ff != null) await financialAffairsRepository.UpdateFinancialAffairsAsync(ff.Id, Email, Address, MobilePhone);
            else if (ss != null) await studentAffairsRepository.UpdateStudentAffairsAsync(ss.Id, Email, Address, MobilePhone);
            else if (adv != null) await advisorRepository.UpdateAdvisorAsync(adv.Id, Email, Address, MobilePhone);
            else if (adm != null) await adminRepository.UpdateAdminAsync(adm.Id, Email, Address, MobilePhone);
            else if (followup != null) await followUpRepository.UpdateFollowUpAsync(followup.Id, Email, Address, MobilePhone);
            return RedirectToAction("ShowProfile");
        }
    }
}
