using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniAppJuanTemplate.Data;
using MiniAppJuanTemplate.Models;
using MiniAppJuanTemplate.ViewModels;


namespace MiniAppJuanTemplate.Controllers
{
    public class ProductController : Controller
    {
        private readonly JuanAppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ProductController(JuanAppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index(int? id)
        {
            return View();
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if(id is null)
            {
                return NotFound();
            }
            var user= await _userManager.GetUserAsync(User);
            if (user is not null)
            {
               var vm=getProductDetailVm((int)id,user.Id);
                if(vm.Product is null)
                {
                    return BadRequest();
                }
                return View(vm);
            }
            else
            {
                var vm = getProductDetailVm((int)id);
                if(vm.Product is null)
                {
                    return NotFound();
                }
                return View(vm);
            }
        }
    
        private ProductDetailVm getProductDetailVm(int productId, string userId)
        {
            var existProd = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductTags)
                .ThenInclude(pt => pt.Tag)
                .Include(p => p.ProductSizes)
                .ThenInclude(ps => ps.Size)
                .Include(p => p.ProductComments)
                .ThenInclude(pc => pc.AppUser)
                .FirstOrDefault(p => p.Id == productId);
            ProductDetailVm productDetailVm = new ProductDetailVm()
            {
                Product = existProd,
                RelatedProducts = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Where(p => p.Category.Id == existProd.Category.Id && p.Id != existProd.Id)
                .Take(5)
                .ToList(),
                HasCommentUser = _context.ProductComments.Any(x => x.ProductId == productId && x.AppUserId == userId && x.Status != CommentStatus.Rejected),
            };
            productDetailVm.TotalCommentsCount = existProd.ProductComments.Count(x => x.Id != existProd.Id);
            productDetailVm.AvarageRate = productDetailVm.TotalCommentsCount > 0 ? (int)_context.ProductComments.Where(x => x.ProductId == existProd.Id).Average(x => x.Rate) : 0;
            return productDetailVm;
        }

        private ProductDetailVm getProductDetailVm(int productId)
        {
            var existProd=_context.Products
                .Include(p=>p.Category)
                .Include(p=>p.ProductImages)
                .Include(p=>p.ProductTags)
                .ThenInclude(pt=>pt.Tag)
                .Include(p=>p.ProductSizes)
                .ThenInclude(ps=>ps.Size)
                .Include(p=>p.ProductComments)
                .ThenInclude(pc=>pc.AppUser)
                .FirstOrDefault(p=>p.Id == productId);
            ProductDetailVm productDetailVm = new ProductDetailVm()
            {
                Product = existProd,
                RelatedProducts = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Where(p => p.Category.Id == existProd.Category.Id && p.Id != existProd.Id)
                .Take(5)
                .ToList(),
                HasCommentUser = _context.ProductComments.Any(x => x.ProductId == productId && x.Status != CommentStatus.Rejected),
            };
            productDetailVm.TotalCommentsCount=existProd.ProductComments.Count(x=>x.Id!=existProd.Id);
            productDetailVm.AvarageRate = productDetailVm.TotalCommentsCount > 0 ? (int)_context.ProductComments.Where(x => x.ProductId == existProd.Id).Average(x => x.Rate) : 0;
            return productDetailVm;
        }

        public async Task<IActionResult> AddComment(ProductComment productComment)
        {

            if (!_context.Products.Any(b => b.Id == productComment.ProductId))
            {
                return RedirectToAction("notfound", "error");
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null || !await _userManager.IsInRoleAsync(user, "member"))
            {
                var returnUrl = Url.Action("Detail", "Product", new { id = productComment.Product?.Id }) ?? "/";
                return RedirectToAction("Login", "Account", returnUrl);
            }
            var productvm = getProductDetailVm(productComment.ProductId, user.Id);
            productvm.ProductComment = productComment;

            if (!ModelState.IsValid) return View("Detail", productvm);
            productComment.AppUserId = user.Id;
           _context.ProductComments.Add(productComment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Detail", new { id = productComment.ProductId});
        }

    }
}
