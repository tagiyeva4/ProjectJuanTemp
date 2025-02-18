using Humanizer.Localisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniAppJuanTemplate.Data;
using MiniAppJuanTemplate.Helpers;
using MiniAppJuanTemplate.Models;

namespace MiniAppJuanTemplate.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "admin,superadmin")]
    public class CategoryController : Controller
    {
        private readonly   JuanAppDbContext _juanAppDbContext;

        public CategoryController(JuanAppDbContext juanAppDbContext)
        {
            _juanAppDbContext = juanAppDbContext;
        }

        public IActionResult Index(int page = 1, int take = 2)
        {
            var query = _juanAppDbContext.Category.Include(c => c.Products).OrderByDescending(c=>c.Id);
            PaginatedList<Category> paginatedlist = PaginatedList<Category>.Create(query, take, page);
            return View(paginatedlist);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_juanAppDbContext.Category.Any(c => c.Name.Trim().ToLower() == category.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("Name", "This category already exist");
                return View();
            }
            _juanAppDbContext.Category.Add(category);
            _juanAppDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
           Category category = _juanAppDbContext.Category.Include(g => g.Products).FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_juanAppDbContext.Category.Any(c => c.Name.Trim().ToLower() == category.Name.Trim().ToLower() && c.Id != category.Id))
            {
                ModelState.AddModelError("Name", "This genre already exist");
                return View();
            }
           Category existCategory =_juanAppDbContext.Category.Include(c => c.Products).FirstOrDefault(c => c.Id == category.Id);
            if (category == null)
            {
                return NotFound();
            }
            existCategory.Name = category.Name;
            _juanAppDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Category? category = _juanAppDbContext.Category.Include(c =>c.Products ).FirstOrDefault(c=> c.Id == id);

            if (category == null)
            {
                return NotFound();
            }
            _juanAppDbContext.Category.Remove(category);
            _juanAppDbContext.SaveChanges();

            return RedirectToAction("Index","Category");
        }
        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Category category = _juanAppDbContext.Category.Include(c =>c.Products).FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
    }
}
