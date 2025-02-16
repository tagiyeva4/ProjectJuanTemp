using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MiniAppJuanTemplate.Data;
using MiniAppJuanTemplate.ViewModels;
using System.Text.Json;

namespace MiniAppJuanTemplate.Services
{
    public class LayoutServices
    {
        private readonly JuanAppDbContext _juanAppDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;

        public LayoutServices(JuanAppDbContext juanAppDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _juanAppDbContext = juanAppDbContext;
            _httpContextAccessor = httpContextAccessor;
            _httpContext = httpContextAccessor.HttpContext;
            
        }
        public Dictionary<string,string> GetSettings()
        {
            return _juanAppDbContext.Settings.ToDictionary(s=>s.Key,s=>s.Value);
        }

        public List<BasketItemVm> GetUserBasketItems()
        {
            List<BasketItemVm> list;

            var basket = _httpContext.Request.Cookies["basket"];
            if (basket != null)
            {
                list = JsonSerializer.Deserialize<List<BasketItemVm>>(basket);
            }
            else
            {
                list = new();
            }
            var user = _juanAppDbContext.Users
                .Include(u => u.BasketItems)
                .ThenInclude(pi => pi.Product)
                .FirstOrDefault(u => u.UserName == _httpContext.User.Identity.Name);
            

            if (user != null)
            {
                foreach (var dbBasketItem in user.BasketItems)
                {
                    if (!list.Any(b => b.Id == dbBasketItem.ProductId))
                    {
                        BasketItemVm basketItemVm = new BasketItemVm();
                        basketItemVm.Id = dbBasketItem.ProductId;
                        basketItemVm.Name = dbBasketItem.Product.Name;
                        basketItemVm.MainImage = dbBasketItem.Product.MainImage;
                        if (dbBasketItem.Product.DiscountPercentege > 0)
                        {
                            basketItemVm.Price = dbBasketItem.Product.CostPrice - ((dbBasketItem.Product.CostPrice * dbBasketItem.Product.DiscountPercentege) / 100);
                        }
                        else
                        {
                            basketItemVm.Price = dbBasketItem.Product.CostPrice;
                        }
                        basketItemVm.Count = dbBasketItem.Count;
                        list.Add(basketItemVm);
                    }
                }
            }
            foreach (var item in list)
            {
                var existProduct =_juanAppDbContext.Products.Include(p=>p.ProductImages).FirstOrDefault(p => p.Id == item.Id);
                item.Name = existProduct.Name;
                item.MainImage = existProduct.MainImage;
                item.Price = existProduct.CostPrice-((existProduct.CostPrice*existProduct.DiscountPercentege)/100);
            }


            _httpContext.Response.Cookies.Append("basket", JsonSerializer.Serialize(list));

            return list;
        }

    }
}
