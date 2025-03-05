using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MiniAppJuanTemplate.Areas.Manage.ViewModels;
using MiniAppJuanTemplate.Data;
using MiniAppJuanTemplate.Helpers;
using MiniAppJuanTemplate.Models;
using MiniAppJuanTemplate.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MiniAppJuanTemplate.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "admin,superadmin")]
    public class ProductController : Controller
    {
        private readonly JuanAppDbContext _juanAppDbContext;
        private readonly IWebHostEnvironment _env;
        private readonly EmailService _emailService;
        public ProductController(JuanAppDbContext juanAppDbContext, IWebHostEnvironment env, EmailService emailService)
        {
            _juanAppDbContext = juanAppDbContext;
            _env = env;
            _emailService = emailService;
        }

        public IActionResult Index(int page = 1, int take = 2)
        {
            var query = _juanAppDbContext.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductTags).ThenInclude(t => t.Tag)
                .Include(p => p.ProductSizes).ThenInclude(s => s.Size).OrderByDescending(p => p.Id);
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
        public IActionResult Create(ProductCreateViewModel viewModel)
        {
            ViewBag.Tags = _juanAppDbContext.Tags.ToList();
            ViewBag.Sizes = _juanAppDbContext.Sizes.ToList();
            ViewBag.Category = _juanAppDbContext.Category.ToList();

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            if (!_juanAppDbContext.Category.Any(c => c.Id == viewModel.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Category not found");
                return View(viewModel);
            }


            Product product = new()
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                CategoryId = viewModel.CategoryId,
                Photos = viewModel.Photos,
                MainPhoto = viewModel.MainPhoto,
                DiscountPercentege = viewModel.DiscountPercentege,
                CostPrice = viewModel.CostPrice,
                IsStock = viewModel.IsStock,
                IsNew = viewModel.IsNew,
                Rate = viewModel.Rate,
                ProductImages = [],
                ProductSizes = [],
                ProductTags = []

            };
            foreach (var tagId in viewModel.TagIds)
            {
                if (!_juanAppDbContext.Tags.Any(t => t.Id == tagId))
                {
                    ModelState.AddModelError("TagIds", "There is no tag in this id...");
                    return View(viewModel);
                }
                ProductTag productTag = new ProductTag();
                productTag.TagId = tagId;
                productTag.Product = product;
                product.ProductTags.Add(productTag);
            }
            foreach (var sizeId in viewModel.SizeIds)
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

            var files = viewModel.Photos;
            if (files.Length > 0)
            {
                foreach (var file in files)
                {
                    ProductImage productImage = new ProductImage();
                    productImage.Name = file.SaveImage(_env.WebRootPath, "assets/img/product");
                    product.ProductImages.Add(productImage);
                }
            }

            string mainImagePath = viewModel.MainPhoto.SaveImage(_env.WebRootPath, "assets/img/product");

            product.MainImage = mainImagePath;

            _juanAppDbContext.Products.Add(product);
            _juanAppDbContext.SaveChanges();

            string? url = Url.Action("Detail", "Shop", new { id = product.Id }, Request.Protocol);
            using StreamReader reader = new StreamReader("wwwroot/templates/subscribeemail.html");
            var body = reader.ReadToEnd();
           

            body = body.Replace("{{PRODUCT_LINK}}", url);

            var subscribers = _juanAppDbContext.SubscribeEmails.ToList();

            foreach (var subscriber in subscribers)
            {
                _emailService.SendEmail(subscriber.Email, "New Product", body);
            }

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
        public IActionResult DeleteBookImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productImage = _juanAppDbContext.ProductImages.Find(id);
            if (productImage is null)
            {
                return NotFound();
            }

            _juanAppDbContext.ProductImages.Remove(productImage);
            _juanAppDbContext.SaveChanges();
            return RedirectToAction("Edit", new { id = productImage.ProductId });
        }
        public IActionResult Edit(int? id)
        {
            ViewBag.Tags = _juanAppDbContext.Tags.ToList();
            ViewBag.Sizes = _juanAppDbContext.Sizes.ToList();
            ViewBag.Category = _juanAppDbContext.Category.ToList();
            if (id == null)
            {
                return NotFound();
            }
            var product = _juanAppDbContext.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductTags).ThenInclude(t => t.Tag)
                .Include(p => p.ProductSizes).ThenInclude(s => s.Size)
                .FirstOrDefault(p => p.Id == id);
            if (product is null)
            {
                return NotFound();
            }
            product.TagIds = product.ProductTags.Select(x => x.TagId).ToList();
            product.SizeIds = product.ProductSizes.Select(x => x.SizeId).ToList();
            ProductUpdateViewModel productUpdateViewModel = new()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CostPrice = product.CostPrice,
                DiscountPercentege = product.DiscountPercentege,
                IsStock = product.IsStock,
                IsNew = product.IsNew,
                Rate=product.Rate,
                CategoryId = product.CategoryId,
                TagIds = product.TagIds,
                SizeIds = product.SizeIds,
                Photos = product.Photos,
                MainPhoto = product.MainPhoto,
                MainImage = product.MainImage,
                ProductImages = product.ProductImages

            };
            return View(productUpdateViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductUpdateViewModel productUpdateViewModel)
        {

            ViewBag.Tags = _juanAppDbContext.Tags.ToList();
            ViewBag.Sizes = _juanAppDbContext.Sizes.ToList();
            ViewBag.Category = _juanAppDbContext.Category.ToList();
            if (!ModelState.IsValid)
            {
                return View();
            }
            var existProduct = _juanAppDbContext.Products.Find(productUpdateViewModel.Id);
            if (existProduct is null)
            {
                return NotFound();
            }
            if (existProduct.CategoryId == productUpdateViewModel.CategoryId)
            {
                if (!_juanAppDbContext.Category.Any(c => c.Id == productUpdateViewModel.CategoryId))
                {
                    ModelState.AddModelError("CategoryId", "Category not found");
                }
            }

            var files = productUpdateViewModel.Photos;
            if (files != null)
            {
                foreach (var file in files)
                {
                    ProductImage productImage = new ProductImage();
                    productImage.Name = file.SaveImage(_env.WebRootPath, "assets/img/product");
                    existProduct.ProductImages.Add(productImage);
                }
            }

            if (productUpdateViewModel.MainPhoto != null)
            {
                string mainImageName = productUpdateViewModel.MainPhoto.SaveImage(_env.WebRootPath, "assets/img/product");
                existProduct.MainImage = mainImageName;
            }
            List<ProductTag> productTags = new List<ProductTag>();
            foreach (var tagId in productUpdateViewModel.TagIds.ToList())
            {
                if (!_juanAppDbContext.Tags.Any(t => t.Id == tagId))
                {
                    ModelState.AddModelError("TagIds", "There is no tag in this id...");
                    return View();
                }
                ProductTag productTag = new ProductTag();
                productTag.TagId = tagId;
                productTag.Product = existProduct;
                productTags.Add(productTag);
            }
            var existProductTags = _juanAppDbContext.ProductTags.Where(pt => pt.ProductId == existProduct.Id).ToList();
            foreach (var prodTag in existProductTags)
            {
                _juanAppDbContext.ProductTags.Remove(prodTag);
            }
            List<ProductSize> productSizes = new List<ProductSize>();
            foreach (var sizeId in productUpdateViewModel.SizeIds.ToList())
            {
                if (!_juanAppDbContext.Sizes.Any(s => s.Id == sizeId))
                {
                    ModelState.AddModelError("SizeIds", "There is no size in this id...");
                    return View();
                }
                ProductSize productSize = new ProductSize();
                productSize.SizeId = sizeId;
                productSize.Product = existProduct;
                productSizes.Add(productSize);
            }
            var existProductSizes = _juanAppDbContext.ProductSizes.Where(pt => pt.ProductId == existProduct.Id).ToList();
            foreach (var productSize in existProductSizes)
            {
                _juanAppDbContext.ProductSizes.Remove(productSize);
            }

            existProduct.ProductTags = productTags;
            existProduct.ProductSizes = productSizes;
            existProduct.Name = productUpdateViewModel.Name;
            existProduct.Description = productUpdateViewModel.Description;
            existProduct.CategoryId = productUpdateViewModel.CategoryId;
            existProduct.Photos = productUpdateViewModel.Photos;
            existProduct.IsStock = productUpdateViewModel.IsStock;
            existProduct.IsNew = productUpdateViewModel.IsNew;
            existProduct.CostPrice = productUpdateViewModel.CostPrice;
            existProduct.DiscountPercentege = productUpdateViewModel.DiscountPercentege;
            existProduct.Rate = productUpdateViewModel.Rate;
            _juanAppDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        #region
        //public IActionResult Edit(Product product)
        //{
        //    ViewBag.Tags = _juanAppDbContext.Tags.ToList();
        //    ViewBag.Sizes = _juanAppDbContext.Sizes.ToList();
        //    ViewBag.Category = _juanAppDbContext.Category.ToList();
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }
        //    var existProduct = _juanAppDbContext.Products.Find(product.Id);
        //    if (existProduct is null)
        //    {
        //        return NotFound();
        //    }
        //    if (existProduct.CategoryId == product.CategoryId)
        //    {
        //        if (!_juanAppDbContext.Category.Any(c => c.Id == product.CategoryId))
        //        {
        //            ModelState.AddModelError("CategoryId", "Category not found");
        //        }
        //    }

        //    var files = product.Photos;
        //    if (files != null)
        //    {
        //        foreach (var file in files)
        //        {
        //            ProductImage productImage = new ProductImage();
        //            productImage.Name = file.SaveImage(_env.WebRootPath, "assets/img/product");
        //            existProduct.ProductImages.Add(productImage);
        //        }
        //    }
        //    List<ProductTag> productTags = new List<ProductTag>();
        //    foreach (var tagId in product.TagIds.ToList())
        //    {
        //        if (!_juanAppDbContext.Tags.Any(t => t.Id == tagId))
        //        {
        //            ModelState.AddModelError("TagIds", "There is no tag in this id...");
        //            return View();
        //        }
        //        ProductTag productTag = new ProductTag();
        //        productTag.Id = tagId;
        //        productTag.Product = existProduct;
        //        productTags.Add(productTag);
        //    }
        //    var existProductTags = _juanAppDbContext.ProductTags.Where(pt => pt.ProductId == existProduct.Id).ToList();
        //    foreach (var prodTag in existProductTags)
        //    {
        //        _juanAppDbContext.ProductTags.Remove(prodTag);
        //    }
        //    List<ProductSize> productSizes = new List<ProductSize>();
        //    foreach (var sizeId in product.SizeIds.ToList())
        //    {
        //        if (!_juanAppDbContext.Sizes.Any(s => s.Id == sizeId))
        //        {
        //            ModelState.AddModelError("SizeIds", "There is no size in this id...");
        //            return View();
        //        }
        //        ProductSize productSize = new ProductSize();
        //        productSize.Id = sizeId;
        //        productSize.Product = existProduct;
        //        productSizes.Add(productSize);
        //    }
        //    var existProductSizes = _juanAppDbContext.ProductSizes.Where(pt => pt.ProductId == existProduct.Id).ToList();
        //    foreach (var productSize in existProductSizes)
        //    {
        //        _juanAppDbContext.ProductSizes.Remove(productSize);
        //    }

        //    existProduct.ProductTags = productTags;
        //    existProduct.ProductSizes = productSizes;
        //    existProduct.Name = product.Name;
        //    existProduct.Description = product.Description;
        //    existProduct.CategoryId = product.CategoryId;
        //    existProduct.Photos = product.Photos;
        //    existProduct.IsStock = product.IsStock;
        //    existProduct.IsNew = product.IsNew;
        //    existProduct.CostPrice = product.CostPrice;
        //    existProduct.DiscountPercentege = product.DiscountPercentege;
        //    existProduct.Rate = product.Rate;
        //    existProduct.ProductImages = product.ProductImages;
        //    _juanAppDbContext.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        #endregion
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product? product = _juanAppDbContext.Products.FirstOrDefault(t => t.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _juanAppDbContext.Products.Remove(product);
            _juanAppDbContext.SaveChanges();

            return RedirectToAction("Index", "Product");
        }
    }
}
