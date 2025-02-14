using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

using MiniAppJuanTemplate.Data;
using MiniAppJuanTemplate.Models;
using MiniAppJuanTemplate.Services;
using MiniAppJuanTemplate.ViewModels;

namespace MiniAppJuanTemplate.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly JuanAppDbContext _juanAppDbContext;
        private readonly EmailService _emailService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, JuanAppDbContext juanAppDbContext, EmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _juanAppDbContext = juanAppDbContext;
            _emailService = emailService;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterVm userRegisterVm)
        {
            if (!ModelState.IsValid) return View();
            AppUser? user = await _userManager.FindByNameAsync(userRegisterVm.UserName);

            if (user != null)
            {
                ModelState.AddModelError("", "This username is already exist");
                return View();
            }
            user = new()
            {
                UserName = userRegisterVm.UserName,
                Email = userRegisterVm.Email,
                FullName = userRegisterVm.FullName,
            };
            var result = await _userManager.CreateAsync(user, userRegisterVm.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(user, "member");

            if (userRegisterVm.Subscribe == true)
            {
                if (_juanAppDbContext.SubscribeEmails.Any(e => e.Email == userRegisterVm.Email))
                {
                    return View();
                }
                SubscribeEmail subscribeEmail = new()
                {
                    Email = userRegisterVm.Email,
                };
                _juanAppDbContext.SubscribeEmails.Add(subscribeEmail);
            }

            //send email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var url = Url.Action("VerifyEmail", "Account", new { email = user.Email, token }, Request.Scheme);

            //send email
            using StreamReader reader = new StreamReader("wwwroot/templates/emailconfirm.html");
            var body = reader.ReadToEnd();
            body = body.Replace("{{{url}}}", url);
            body = body.Replace("{{{username}}}", user.UserName);
            _emailService.SendEmail(user.Email, "Verify Email Address for Pustok", body);
            TempData["Succsess"] = "Email successfully sended to" + user.Email;

            return RedirectToAction("Login");
        }

        public async Task<IActionResult> VerifyEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !await _userManager.IsInRoleAsync(user, "member")) return RedirectToAction("notfound", "error");
            await _userManager.ConfirmEmailAsync(user, token);
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginVm userLoginVm, string? returnUrl)
        {
            TempData["Succsess"] = "Email successfully sended to";
            if (!ModelState.IsValid) return View();
            AppUser? user = await _userManager.FindByNameAsync(userLoginVm.UserNameOrEmail);
            if (user == null || !await _userManager.IsInRoleAsync(user, "member"))
            {
                user = await _userManager.FindByEmailAsync(userLoginVm.UserNameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "Username or email is invalid..");
                    return View();
                }
            }

            var result = await _signInManager.PasswordSignInAsync(user, userLoginVm.Password, userLoginVm.RememberMe, true);
            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Email is not confirmed..");
                return View();
            }
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Account is locked out..");
                return View();
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or email or password is invalid..");
                return View();
            }
            HttpContext.Response.Cookies.Append("basket", "");

            return returnUrl != null ? Redirect(returnUrl) : RedirectToAction("Index", "Home");
        }
        [Authorize(Roles = "member")]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        [Authorize(Roles = "member")]
        public async Task<IActionResult> Profile(string tab = "dashboard")
        {
            ViewBag.Tab = tab;
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            UserAccountVm userAccount = new UserAccountVm();
            userAccount.UserAccountUpdatVm = new()
            {
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.UserName,
            };
            //userprofileVm.Orders = _pustokAppDbContext.Orders
            //    .Include(o => o.AppUser)
            //    .Where(o => o.AppUserId == user.Id)
            //    .ToList();
            return View(userAccount);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> Profile(UserAccountUpdatVm userAccountUpdatVm, string tab = "accountdetail")
        {
            ViewBag.Tab = tab;
            UserAccountVm userAccountVm = new();
            userAccountVm.UserAccountUpdatVm = userAccountUpdatVm;

            if (!ModelState.IsValid) return View(userAccountVm);
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            user.FullName = userAccountUpdatVm.FullName;
            user.UserName = userAccountUpdatVm.UserName;
            user.Email = userAccountUpdatVm.Email;

            if (userAccountUpdatVm.NewPassword != null)
            {
                if (userAccountUpdatVm.CurrentPasword == null)
                {
                    ModelState.AddModelError("CurrentPasword", "CurrentPasword is required");
                    return View(userAccountVm);
                }
                var response = await _userManager.ChangePasswordAsync(user, userAccountUpdatVm.CurrentPasword, userAccountUpdatVm.NewPassword);
                if (!response.Succeeded)
                {
                    foreach (var error in response.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(userAccountVm);
                }
            }
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(userAccountVm);
            }
            await _signInManager.SignInAsync(user, true);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVm forgotPasswordVm)
        {
            if (!ModelState.IsValid) return View();
            var user = await _userManager.FindByEmailAsync(forgotPasswordVm.Email);
            if (user == null || !await _userManager.IsInRoleAsync(user, "member")) return RedirectToAction("notfound", "error");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = Url.Action("VerifyPassword", "Account", new { email = user.Email, token }, Request.Scheme);

            //send email
            using StreamReader reader = new StreamReader("wwwroot/templates/resetpassword.html");
            var body = reader.ReadToEnd();
            body = body.Replace("{{{url}}}", url);
            body = body.Replace("{{{username}}}", user.UserName);
            _emailService.SendEmail(user.Email, "Reset Password for Login", body);
            TempData["Succsess"] = "Email successfully sended to " + user.Email;
            return View();
        }
        public async Task<IActionResult> VerifyPassword(string token, string email)
        {
            TempData["token"] = token;
            TempData["email"] = email;
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !await _userManager.IsInRoleAsync(user, "member")) return RedirectToAction("notfound", "error");
            if (!await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token))
            {
                return RedirectToAction("notfound", "error");
            }

            return RedirectToAction("ResetPassword");
        }

        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(PasswordResetVm passwordResetVm)
        {
            TempData["token"] = passwordResetVm.Token;
            TempData["email"] = passwordResetVm.Email;
            if (!ModelState.IsValid) return View();
            var user = await _userManager.FindByEmailAsync(passwordResetVm.Email);

            if (user == null || !await _userManager.IsInRoleAsync(user, "member")) return View();

            if (!await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", passwordResetVm.Token))
            {
                return RedirectToAction("notfound", "error");
            }

            var result = await _userManager.ResetPasswordAsync(user, passwordResetVm.Token, passwordResetVm.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(" ", error.Description);
                }
                return View();
            }
            return RedirectToAction("Login");
        }
    }
}
