using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiniAppJuanTemplate.Data;
using MiniAppJuanTemplate.ViewModels;

namespace MiniAppJuanTemplate.Controllers
{
    public class ShopController : Controller
    {
        private readonly JuanAppDbContext _juanAppDbContext;

        public ShopController(JuanAppDbContext juanAppDbContext)
        {
            _juanAppDbContext = juanAppDbContext;
        }

        public IActionResult Index(int? categoryId = null, List<int>? tagIds = null, List<int>? sizeIds = null, string sort = "Name(A-Z)", int? minPrice = null, int? maxPrice = null)
        {
            ShopVm shopVm = new ShopVm();
            shopVm.Tags=_juanAppDbContext.Tags.ToList();
            shopVm.Sizes = _juanAppDbContext.Sizes.ToList();
            shopVm.Categories = _juanAppDbContext.Category.ToList();
            shopVm.ProductSizes = _juanAppDbContext.ProductSizes.ToList();
            var query = _juanAppDbContext.Products
                .Include(p => p.Category)
                .Include(p => p.ProductSizes).ThenInclude(ps => ps.Size)
                .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
               .AsQueryable();
            if(categoryId != null)
            {
                query=query.Where(p=>p.CategoryId == categoryId);   
            }
            if(tagIds != null)
            {
                query = query.Where(p => p.ProductTags.Any(pt => tagIds.Contains(pt.TagId)));
            }
            if(sizeIds != null)
            {
                query = query.Where(p => p.ProductSizes.Any(ps => sizeIds.Contains(ps.SizeId)));
            }
            if (minPrice != null && maxPrice != null)
            {
                query = query.Where(b => b.DiscountPercentege > 0 ? (b.CostPrice - ((b.CostPrice * b.DiscountPercentege) / 100) >= minPrice && b.CostPrice - ((b.CostPrice * b.DiscountPercentege) / 100) <= maxPrice) : (b.CostPrice >= minPrice && b.CostPrice <= maxPrice));

            }
            switch (sort)
            {
                case "Name(Z-A)":
                    query = query.OrderByDescending(b => b.Name);
                    break;
                case "Price(High>Low)":
                    query = query.OrderByDescending(b => b.DiscountPercentege > 0 ? b.CostPrice - ((b.CostPrice * b.DiscountPercentege) / 100) : b.CostPrice);
                    break;
                case "Price(Low<High)":
                    query = query.OrderBy(b => b.DiscountPercentege > 0 ? b.CostPrice - ((b.CostPrice * b.DiscountPercentege) / 100) : b.CostPrice);
                    break;
                default:
                    query = query.OrderBy(b => b.Name);
                    break;

            }
            shopVm.Products=query.ToList();
            ViewBag.CategoryId = categoryId;
            ViewBag.TagIds=tagIds;
            ViewBag.SizeIds=sizeIds;
            ViewBag.Sort=sort;
            ViewBag.SortList = new List<SelectListItem>()
            {
                new SelectListItem(){Text="Name(A-Z)",Value="AtoZ",Selected=sort=="Name(A-Z)"},
                new SelectListItem(){Text="Name(Z-A)",Value="ZtoA",Selected=sort=="Name(Z-A)"},
                new SelectListItem(){Text="Price(High>Low)",Value="Price(High>Low)",Selected=sort=="Price(High>Low)"},
                new SelectListItem(){Text="Price(Low<High)",Value="Price(Low<High)",Selected=sort=="Price(Low<High)"}
            };
            ViewBag.MinPrice =_juanAppDbContext.Products.Min(b => b.CostPrice);
            ViewBag.MaxPrice = _juanAppDbContext.Products.Max(b => b.CostPrice);
            ViewBag.SelectedMinPrice = minPrice ?? ViewBag.MinPrice;
            ViewBag.SelectedMaxPrice = maxPrice ?? ViewBag.MaxPrice;
            return View(shopVm);
        }
    }
}
