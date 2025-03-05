using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniAppJuanTemplate.Data;
using MiniAppJuanTemplate.Models;
using MiniAppJuanTemplate.ViewModels;

namespace MiniAppJuanTemplate.Controllers
{
    public class OrderController : Controller
    {
        private readonly JuanAppDbContext _juanAppDbContext;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(JuanAppDbContext juanAppDbContext, UserManager<AppUser> userManager)
        {
            _juanAppDbContext = juanAppDbContext;
            _userManager = userManager;
        }
        [Authorize(Roles ="member")]
        public IActionResult Cancel(int orderId)
        {
            var order = _juanAppDbContext.Orders
                .Where(o => o.AppUserId == _userManager.GetUserId(User))
                .FirstOrDefault(o => o.Id == orderId);
            order.OrderStatus = OrderStatus.Cancelled;
            _juanAppDbContext.SaveChanges();
            return RedirectToAction("Profile", "Account", new { tab = "orders" });

        }
        [Authorize(Roles = "member")]
        public IActionResult GetOrderItems(int orderId)
        {
            var order = _juanAppDbContext.Orders
               .Where(o => o.AppUserId == _userManager.GetUserId(User))
              .Include(o => o.OrderItems)
              .ThenInclude(oi => oi.Product)
              .FirstOrDefault(o => o.Id == orderId);
            return PartialView("_OrderItemsPartial", order);

        }
        [Authorize(Roles = "member")]
        public IActionResult CheckOut()
        {
            var user = _userManager.Users
                .Include(u => u.BasketItems)
                .ThenInclude(pi => pi.Product)
                .FirstOrDefault(u => u.UserName == User.Identity.Name);



            CheckOutVm checkOutVm = new CheckOutVm();
            checkOutVm.CheckoutItemVms = user.BasketItems.Select(p => new CheckoutItemVm
            {
                ProductName = p.Product?.Name ?? "product",
                TotalItemPrice = p.Product.DiscountPercentege > 0 ? (p.Product.CostPrice - (p.Product.CostPrice * p.Product.DiscountPercentege) / 100) * p.Count : p.Product.CostPrice * p.Count,
                Count = p.Count,
            }).ToList();

            return View(checkOutVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "member")]
        public IActionResult CheckOut(OrderVm orderVm)
        {
            var user = _userManager.Users
                .Include(u => u.BasketItems)
                .ThenInclude(pi => pi.Product)
                .FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!ModelState.IsValid)
            {
                CheckOutVm checkOutVm = new CheckOutVm();
                checkOutVm.CheckoutItemVms = user.BasketItems.Select(p => new CheckoutItemVm
                {
                    ProductName = p.Product.Name,
                    TotalItemPrice = p.Product.DiscountPercentege > 0 ? (p.Product.CostPrice - (p.Product.CostPrice * p.Product.DiscountPercentege) / 100) * p.Count : p.Product.CostPrice * p.Count,
                    Count = p.Count,
                }).ToList();
                checkOutVm.OrderVm = orderVm;
                return View(checkOutVm);
            }
            Order order = new Order();
            order.AppUserId = user.Id;
            order.Country = orderVm.Country;
            order.PhoneNumber = orderVm.PhoneNumber;
            order.CompanyName = orderVm.CompanyName;
            order.OrderStatus = OrderStatus.Pending;
            order.State = orderVm.State;
            order.StreetAddress = orderVm.StreetAddress;
            order.TownCity = orderVm.TownCity;
            order.TotalPrice = user.BasketItems.Sum(p => p.Product.DiscountPercentege > 0 ? (p.Product.CostPrice-(p.Product.CostPrice*p.Product.DiscountPercentege)/100)*p.Count : p.Product.CostPrice * p.Count);
            order.ZipCode = orderVm.ZipCode;
            order.OrderItems = user.BasketItems.Select(p => new OrderItem
            {
                ProductId = p.ProductId,
                Count = p.Count,
            }).ToList();
            _juanAppDbContext.Orders.Add(order);
            _juanAppDbContext.BasketItems.RemoveRange(user.BasketItems);
            _juanAppDbContext.SaveChanges();
            HttpContext.Response.Cookies.Delete("basket");
            return RedirectToAction("Profile", "Account", new { tab = "orders" });
        }

    }
}
