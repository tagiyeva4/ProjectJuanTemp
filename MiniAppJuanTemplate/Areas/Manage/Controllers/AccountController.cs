using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniAppJuanTemplate.Areas.Manage.ViewModels;
using MiniAppJuanTemplate.Models;

namespace MiniAppJuanTemplate.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> CreateAdminUser()
        {
            AppUser user = new AppUser
            {
                UserName = "_superadmin",
                Email = "superadmin@gmail.com",
                FullName="Superadmin",
            };
            IdentityResult identityResult = await _userManager.CreateAsync(user,"Salam123!");
            await _userManager.AddToRoleAsync(user, "admin");
            return Json(identityResult);
        }
        public async Task<IActionResult> CreateRole()
        {
            await _roleManager.CreateAsync(new IdentityRole("admin"));
            await _roleManager.CreateAsync(new IdentityRole("superadmin"));
            await _roleManager.CreateAsync(new IdentityRole("member"));
            return Content("Roles added..");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AdminLoginVm adminLoginVm, string? returnUrl)
        {
            if (!ModelState.IsValid) return View();
            var user = await _userManager.FindByNameAsync(adminLoginVm.UserName);
            if (user == null || (!await _userManager.IsInRoleAsync(user, "admin") && !await _userManager.IsInRoleAsync(user, "superadmin")))
            {
                ModelState.AddModelError("", "UserName or Password is incorret...");
                return View();
            }
            var result = await _userManager.CheckPasswordAsync(user, adminLoginVm.Password);
            if (!result)
            {
                ModelState.AddModelError("", "UserName or Password is incorret...");
                return View();
            }
            await _signInManager.SignInAsync(user, false);

            return returnUrl != null ? Redirect(returnUrl) : RedirectToAction("Index", "Dashboard");

        }
        public async Task<IActionResult> GetUser()
        {
            var user = await _userManager.GetUserAsync(User);
            return Json(user);
        }
        [Authorize(Roles = "admin,superadmin")]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
