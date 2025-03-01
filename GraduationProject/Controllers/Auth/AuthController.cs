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

       public AuthController(UserManager<GPUser> userManager,SignInManager<GPUser> signInManager, RoleManager<IdentityRole> roleManager
                                 )  // Add EmailSettings to constructor
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
           
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
            return RedirectToAction(nameof(Login));
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
        public IActionResult ShowProfile()
        {
            return View("Profile");
        }
        public IActionResult EditProfile()
        {
            return View("EditProfile");
        }
    }
}
