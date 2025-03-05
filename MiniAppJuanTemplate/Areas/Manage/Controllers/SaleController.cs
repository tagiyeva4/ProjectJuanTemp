using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniAppJuanTemplate.Data;

namespace MiniAppJuanTemplate.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "admin,superadmin")]
    public class SaleController : Controller
    {
        private readonly JuanAppDbContext _juanAppDbContext;

        public SaleController(JuanAppDbContext juanAppDbContext)
        {
            _juanAppDbContext = juanAppDbContext;
        }

        public IActionResult Index()
        {
            var orders = _juanAppDbContext.Orders
             .Include(o => o.AppUser) // İstifadəçi məlumatlarını əlavə etdik
             .Include(o => o.OrderItems) // Sifarişin məhsullarını əlavə etdik
                 .ThenInclude(oi => oi.Product) // Məhsul haqqında məlumatları əlavə etdik (əgər lazımdırsa)
             .ToList();
            return View(orders);
        }
    }
}
