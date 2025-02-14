using Microsoft.AspNetCore.Mvc;
using MiniAppJuanTemplate.Data;
using MiniAppJuanTemplate.ViewModels;

namespace MiniAppJuanTemplate.Controllers
{
    public class BlogController : Controller
    {
        private readonly JuanAppDbContext _juanAppDbContext;

        public BlogController(JuanAppDbContext juanAppDbContext)
        {
            _juanAppDbContext = juanAppDbContext;
        }

        public IActionResult Index()
        {
            BlogVm blogVm = new BlogVm();
            blogVm.Blogs=_juanAppDbContext.Blogs.OrderBy(x=>x.Id).Take(6).ToList();
            blogVm.Tags=_juanAppDbContext.Tags.ToList();
            blogVm.Categories = _juanAppDbContext.Category.ToList();
            blogVm.RecentBlogs=_juanAppDbContext.Blogs.OrderByDescending(x => x.Id).Take(4).ToList();
            return View(blogVm);
        }
        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            BlogDetailVm vm = new BlogDetailVm();
            vm.Tags = _juanAppDbContext.Tags.Take(5).ToList();
            vm.Categories = _juanAppDbContext.Category.ToList();
            vm.Blog = _juanAppDbContext.Blogs.FirstOrDefault(x => x.Id == id);
            vm.RecentBlogs = _juanAppDbContext.Blogs.OrderByDescending(x => x.Id).Take(3).ToList();
            return View(vm);
        }
    }
}
