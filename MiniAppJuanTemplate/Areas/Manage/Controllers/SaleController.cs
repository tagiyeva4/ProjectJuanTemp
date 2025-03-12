using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MiniAppJuanTemplate.Data;
using MiniAppJuanTemplate.Models;

namespace MiniAppJuanTemplate.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "admin,superadmin")]
    public class SaleController(JuanAppDbContext _juanAppDbContext, IHubContext<ChatHub> _hubContext) : Controller
    {
        public IActionResult Index()
        {
            var orders = _juanAppDbContext.Orders
             .Include(o => o.AppUser) // İstifadəçi məlumatlarını əlavə etdik
             .Include(o => o.OrderItems) // Sifarişin məhsullarını əlavə etdik
           .ThenInclude(oi => oi.Product) // Məhsul haqqında məlumatları əlavə etdik (əgər lazımdırsa)
             .ToList();
            return View(orders);
        }
        public IActionResult Accept(int id)
        {
            var data = _juanAppDbContext.Orders.FirstOrDefault(x => x.Id == id);
            data.OrderStatus = OrderStatus.Accepted;
            _juanAppDbContext.SaveChanges();
            var user = _juanAppDbContext.Users.FirstOrDefault(x => x.Id == data.AppUserId);
            _hubContext.Clients.Client(user.ConnectionId).SendAsync("OrderAccepted", data.OrderStatus);
            return RedirectToAction("Index");

        }
        public IActionResult Reject(int id)
        {
            var data = _juanAppDbContext.Orders.FirstOrDefault(x => x.Id == id);
            data.OrderStatus = OrderStatus.Rejected;
            _juanAppDbContext.SaveChanges();
            var user = _juanAppDbContext.Users.FirstOrDefault(x => x.Id == data.AppUserId);
            _hubContext.Clients.Client(user.ConnectionId).SendAsync("OrderRejected", data.OrderStatus);
            return RedirectToAction("Index");

        }
    }
}
