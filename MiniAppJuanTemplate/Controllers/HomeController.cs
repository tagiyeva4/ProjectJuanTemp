using Microsoft.AspNetCore.Mvc;
using MiniAppJuanTemplate.Data;
using MiniAppJuanTemplate.ViewModels;

namespace MiniAppJuanTemplate.Controllers
{
    public class HomeController : Controller
    {
        private readonly JuanAppDbContext _juanAppDbContext;

        public HomeController(JuanAppDbContext juanAppDbContext)
        {
            _juanAppDbContext = juanAppDbContext;
        }

        public IActionResult Index()
        {
            HomeVm homeVm = new HomeVm();
            homeVm.Sliders=_juanAppDbContext.Sliders.ToList();
            homeVm.Services=_juanAppDbContext.HomeServices.ToList();
            homeVm.Brands=_juanAppDbContext.Brands.ToList();
            homeVm.Blogs=_juanAppDbContext.Blogs.ToList();
            homeVm.Products=_juanAppDbContext.Products.ToList();
            homeVm.NewProducts=_juanAppDbContext.Products.Where(p=>p.IsNew==true).ToList();
            return View(homeVm);
        }
    }
}
