﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniAppJuanTemplate.Data;
using MiniAppJuanTemplate.Models;
using MiniAppJuanTemplate.ViewModels;
using Newtonsoft.Json;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

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

        [HttpGet]
        public async Task<IActionResult> RemoveToBasketAsync(int id)
        {

            if (User.Identity?.IsAuthenticated ?? false)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user is null)
                {
                    return NotFound(new { message = "User Not Found!" });
                }

                var basketItem = await _juanAppDbContext.BasketItems
                    .FirstOrDefaultAsync(x => x.AppUserId == user.Id && x.Id == id);

                if (basketItem is null)
                {
                    return NotFound();
                }

                _juanAppDbContext.BasketItems.Remove(basketItem);
                await _juanAppDbContext.SaveChangesAsync();
                return RedirectToAction("Index", "Home");

            }
            else
            {
                List<BasketItemVm> basketItems = [];
                var cookieValue = Request.Cookies["basket"];
                if (cookieValue is not null)
                {
                    basketItems = JsonConvert.DeserializeObject<List<BasketItemVm>>(cookieValue) ?? [];
                }

                var existItem = basketItems.FirstOrDefault(x => x.Id == id);
                if (existItem is null)
                {
                    return NotFound();
                }

                basketItems.Remove(existItem);
                var json = JsonConvert.SerializeObject(basketItems);
                Response.Cookies.Append("basket", json);

            }
            string? returnUrl = Request.Headers["Referer"];

            if (returnUrl is null)
                returnUrl = "/";

            return Redirect(returnUrl);
        }


    }
}
