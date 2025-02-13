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
    //[Authorize(Roles = "admin,superadmin")]
    public class TagController : Controller
    {
        private readonly JuanAppDbContext _juanAppDbContext;

        public TagController(JuanAppDbContext juanAppDbContext)
        {
            _juanAppDbContext = juanAppDbContext;
        }

        public IActionResult Index(int page = 1, int take = 2)
        {
            var query = _juanAppDbContext.Tags.Include(c => c.Products);
            PaginatedList<Tag> paginatedlist = PaginatedList<Tag>.Create(query, take, page);
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
                ModelState.AddModelError("Name", "This genre already exist");
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
            Tag tag = _juanAppDbContext.Tags.Include(t => t.Products).FirstOrDefault(t => t.Id == id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_juanAppDbContext.Tags.Any(t => t.Name.Trim().ToLower() == tag.Name.Trim().ToLower() && t.Id != tag.Id))
            {
                ModelState.AddModelError("Name", "This genre already exist");
                return View();
            }
            Tag existTag = _juanAppDbContext.Tags.Include(t => t.Products).FirstOrDefault(t => t.Id == tag.Id);
            if (tag == null)
            {
                return NotFound();
            }
            existTag.Name =tag.Name;
            _juanAppDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Tag tag = _juanAppDbContext.Tags.Include(t => t.Products).FirstOrDefault(t => t.Id == id);
            if (tag== null)
            {
                return NotFound();
            }
            _juanAppDbContext.Tags.Remove(tag);
            _juanAppDbContext.SaveChanges();

            return Ok();
        }
        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Tag tag = _juanAppDbContext.Tags.Include(t => t.Products).FirstOrDefault(t => t.Id == id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }
    }
}
