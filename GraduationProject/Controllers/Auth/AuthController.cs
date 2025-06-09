using GP.BLL.Interfaces;
using GP.DAL.Context;
using GP.DAL.Models;
using GraduationProject.Controllers.Home;
using GraduationProject.Helpers;
using GraduationProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

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
        private readonly EmailSettings _email;
        private readonly IFollowUpRepository followUpRepository;
       public AuthController(EmailSettings email, IFollowUpRepository _followUpRepository, IAdminRepository _adminRepository, IStudentAffairsRepository _studentAffairsRepository, IFinancialAffairsRepository _financialAffairsRepository, IAdvisorRepository _advisorRepository, IFacultyMemberRepsitory _facultyMemberRepsitory, IStudentRepository _studentRepository, UserManager<GPUser> userManager,SignInManager<GPUser> signInManager, RoleManager<IdentityRole> roleManager
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
            _email = email;
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
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM forgetPassword)
        {
            if (!ModelState.IsValid)
            {
                return View(forgetPassword);
            }

            var user = await _userManager.Users
                         .FirstOrDefaultAsync(u =>
                         u.Admin.MobilePhone == forgetPassword.MobilePhone ||
                         u.Advisor.MobilePhone == forgetPassword.MobilePhone ||
                         u.Student.MobilePhone == forgetPassword.MobilePhone ||
                         u.FacultyMember.MobilePhone == forgetPassword.MobilePhone ||
                         u.StudentAffairs.MobilePhone == forgetPassword.MobilePhone ||
                         u.FinancialAffairs.MobilePhone == forgetPassword.MobilePhone ||
                         u.FollowUp.MobilePhone == forgetPassword.MobilePhone
                         );

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Your mobile Phone not found.");
                return View(forgetPassword);
            }

            if (!await IsRealEmail(forgetPassword.Email))
            {
                ModelState.AddModelError(string.Empty, "Please enter a valid real email (Gmail, Yahoo, etc.).");
                return View(forgetPassword);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetPasswordUrl = Url.Action("NewPassword", "Auth", new { token = token, 
                mobile = forgetPassword.MobilePhone }, Request.Scheme);

            var email = new Email()
            {
                Subject = "Reset Password",
                Recipients = forgetPassword.Email,
                Body = resetPasswordUrl
            };

            _email.SendEmail(email);

            return Json(new { success = true, message = "Email has been sent. Please check your inbox!" });
        }

        private async Task<bool> IsRealEmail(string email)
        {
            return email.EndsWith("@gmail.com") ||
                   email.EndsWith("@yahoo.com") ||
                   email.EndsWith("@outlook.com");
        }


        [HttpGet]
        public IActionResult NewPassword(string mobile, string token)
        {
            TempData["mobile"] = mobile;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NewPassword(ResetPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var mobile = TempData["mobile"] as string;
                var token = TempData["token"] as string;
                var user = await _userManager.Users
                         .FirstOrDefaultAsync(u =>
                         u.Admin.MobilePhone == mobile ||
                         u.Advisor.MobilePhone == mobile ||
                         u.Student.MobilePhone == mobile ||
                         u.FacultyMember.MobilePhone == mobile ||
                         u.StudentAffairs.MobilePhone == mobile ||
                         u.FinancialAffairs.MobilePhone == mobile ||
                         u.FollowUp.MobilePhone == mobile
                         );
                var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
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
            if (f != null) await facultyMemberRepsitory.UpdateFacultyAsync(f.TeacherId, Email, Address, MobilePhone);
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
