using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniAppJuanTemplate.Data;
using MiniAppJuanTemplate.Models;
using MiniAppJuanTemplate.ViewModels;
using System.Text.Json;

namespace MiniAppJuanTemplate.Controllers
{
    public class BasketController : Controller
    {
        private readonly JuanAppDbContext _juanAppDbContext;
        private readonly UserManager<AppUser> _userManager;
        public BasketController(JuanAppDbContext juanAppDbContext, UserManager<AppUser> userManager)
        {
            _juanAppDbContext = juanAppDbContext;
            _userManager = userManager;
        }

        public IActionResult Add(int? id)
        {
            if (id == null) return NotFound();
            var product = _juanAppDbContext.Products
                .Include(p => p.ProductImages)
                .FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            var basket = HttpContext.Request.Cookies["basket"];
            List<BasketItemVm> basketItemVms;
            if (basket == null)
            {
                basketItemVms = new();
            }
            else
            {
                basketItemVms = JsonSerializer.Deserialize<List<BasketItemVm>>(basket);
            }
            var basketItemVm = basketItemVms.FirstOrDefault(p => p.Id == id);
            if (basketItemVm == null)
            {
                BasketItemVm basketItem = new();
                basketItem.Id = product.Id;
                basketItem.Name = product.Name;
                basketItem.MainImage = product.MainImage;
                if (product.DiscountPercentege > 0)
                {
                    basketItem.Price = product.CostPrice - ((product.CostPrice * product.DiscountPercentege) / 100);
                }
                else
                {
                    basketItem.Price = product.CostPrice;
                }
                basketItem.Count = 1;
                basketItemVms.Add(basketItem);
            }
            else
            {
                basketItemVm.Count++;
            }
            if (User.Identity.IsAuthenticated && User.IsInRole("member"))
            {
                var user = _userManager.Users.Include(u => u.BasketItems).FirstOrDefault(u => u.UserName == User.Identity.Name);
                var basketItem = user.BasketItems.FirstOrDefault(p => p.ProductId == id);
                if (basketItem is null)
                {
                    BasketItem newBasketItem = new();
                    newBasketItem.ProductId = product.Id;
                    newBasketItem.Count = 1;
                    newBasketItem.AppUserId = user.Id;
                    user.BasketItems.Add(newBasketItem);
                }
                else
                {
                    basketItem.Count++;
                }
                _juanAppDbContext.SaveChanges();
            }
            HttpContext.Response.Cookies.Append("basket", JsonSerializer.Serialize(basketItemVms));
            return PartialView("_BasketPartial", basketItemVms);
        }
        public IActionResult Index()
        {
            var basket = HttpContext.Request.Cookies["basket"];
            List<BasketItemVm> basketItemVms;
            if (basket == null)
            {
                basketItemVms = new();
            }
            else
            {
                basketItemVms = JsonSerializer.Deserialize<List<BasketItemVm>>(basket);
            }
            return View(basketItemVms);
        }
        //[Authorize(Roles = "member")]
        //public IActionResult DeleteItem(int? id)
        //{
        //    if (id == null) return NotFound();
           
        //}
    }
}
