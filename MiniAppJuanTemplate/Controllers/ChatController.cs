using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniAppJuanTemplate.Models;

namespace MiniAppJuanTemplate.Controllers
{
    public class ChatController(UserManager<AppUser>userManager): Controller
    {
        public IActionResult Index()
        {
            ViewBag.Users = userManager.Users.ToList();
            return View();
        }
    }
}
