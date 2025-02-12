using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniAppJuanTemplate.Data;
using MiniAppJuanTemplate.Helpers;
using MiniAppJuanTemplate.Models;

namespace MiniAppJuanTemplate.Areas.Manage.Controllers
{
    [Area("Manage")]
    //[Authorize(Roles = "admin,superadmin")]
    public class ProductController : Controller
    {
        private readonly JuanAppDbContext _juanAppDbContext;
        private readonly IWebHostEnvironment _env;

        public ProductController(JuanAppDbContext juanAppDbContext, IWebHostEnvironment env = null)
        {
            _juanAppDbContext = juanAppDbContext;
            _env = env;
        }

        //public IActionResult Index(int page = 1, int take = 2)
        //{
        //    var query = _juanAppDbContext.Products
        //        .Include(p => p.Category)
        //        .Include(p => p.ProductImages)
        //        .Include(p => p.ProductSizes)
        //        .Include(p => p.ProductTags);
        //    return View(PaginatedList<Product>.Create(query,take));
                
        //}
        //public IActionResult Create()
        //{

        //}
    }
}
