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

        public IActionResult Index(int page = 1, int take = 2)
        {
            var query = _juanAppDbContext.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductTags).ThenInclude(t => t.Tag)
                .Include(p => p.ProductSizes).ThenInclude(s => s.Size);
            return View(PaginatedList<Product>.Create(query, take, page));
        }
        public IActionResult Create()
        {
            ViewBag.Tags = _juanAppDbContext.Tags.ToList();
            ViewBag.Sizes = _juanAppDbContext.Sizes.ToList();
            ViewBag.Category = _juanAppDbContext.Category.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            ViewBag.Tags = _juanAppDbContext.Tags.ToList();
            ViewBag.Sizes = _juanAppDbContext.Sizes.ToList();
            ViewBag.Category = _juanAppDbContext.Category.ToList();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_juanAppDbContext.Category.Any(c => c.Id == product.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Category not found");
            }
            foreach (var tagId in product.TagIds)
            {
                if (!_juanAppDbContext.Tags.Any(t => t.Id == tagId))
                {
                    ModelState.AddModelError("TagIds", "There is no tag in this id...");
                    return View();
                }
                ProductTag productTag = new ProductTag();
                productTag.TagId = tagId;
                productTag.Product = product;
                product.ProductTags.Add(productTag);
            }
            foreach (var sizeId in product.SizeIds)
            {
                if (!_juanAppDbContext.Tags.Any(t => t.Id == sizeId))
                {
                    ModelState.AddModelError("SizeIds", "There is no size in this id...");
                    return View();
                }
               ProductSize productSize = new ProductSize();
                productSize.SizeId = sizeId;
                productSize.Product = product;
                product.ProductSizes.Add(productSize);
            }

            var files = product.Photos;
            if (files.Length > 0)
            {
                foreach (var file in files)
                {
                   ProductImage productImage = new  ProductImage();
                   productImage.Name = file.SaveImage(_env.WebRootPath, "assets/img/product");
                    product.ProductImages.Add(productImage);
                }
            }
            _juanAppDbContext.Products.Add(product);
            _juanAppDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _juanAppDbContext.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductTags).ThenInclude(t => t.Tag)
                .Include(p => p.ProductSizes).ThenInclude(s => s.Size)
                .FirstOrDefault(x => x.Id == id);
            if (product is null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}
