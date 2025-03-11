using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MiniAppJuanTemplate.Data;
using MiniAppJuanTemplate.ViewModels;

namespace MiniAppJuanTemplate.Controllers
{
    public class HomeController : Controller
    {
        private readonly JuanAppDbContext _juanAppDbContext;
        private readonly IHubContext<ChatHub> _hubContext;

        public HomeController(JuanAppDbContext juanAppDbContext, IHubContext<ChatHub> hubContext)
        {
            _juanAppDbContext = juanAppDbContext;
            this._hubContext = hubContext;
        }

        public IActionResult Search(string search)
        {
            if (search is not null)
            {
                var datas = _juanAppDbContext.Products
                    .Include(p => p.Category)
                    .Include(p => p.ProductTags)
                    .ThenInclude(pt => pt.Tag)
                    .Where(x => x.Name.Contains(search)
                    || x.Category.Name.Contains(search)
                    || x.ProductTags.Any(pt => pt.Tag.Name.Contains(search)))
                    .ToList();
                return PartialView("_SearchPartial", datas);
            }
            else
            {
                return NotFound();
            }
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

        public IActionResult ShowAlert(string userId)
        {
            var user = _juanAppDbContext.Users.Find(userId);
            _hubContext.Clients.Client(user.ConnectionId).SendAsync("ShowAlert", userId);

            return Content("");

        }
    }
}
