﻿using Microsoft.AspNetCore.Mvc;

namespace MiniAppJuanTemplate.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
