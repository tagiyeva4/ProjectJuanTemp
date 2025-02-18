using Humanizer.Localisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniAppJuanTemplate.Areas.Manage.Services.Interfaces;
using MiniAppJuanTemplate.Data;
using MiniAppJuanTemplate.Helpers;
using MiniAppJuanTemplate.Models;

namespace MiniAppJuanTemplate.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "admin,superadmin")]
    public class TagController : Controller
    {
        #region without service
        //private readonly JuanAppDbContext _juanAppDbContext;

        //public TagController(JuanAppDbContext juanAppDbContext)
        //{
        //    _juanAppDbContext = juanAppDbContext;
        //}

        //public IActionResult Index(int page = 1, int take = 2)
        //{
        //    var query = _juanAppDbContext.Tags.Include(c => c.ProductsTags);
        //    PaginatedList<Tag> paginatedlist = PaginatedList<Tag>.Create(query, take, page);
        //    return View(paginatedlist);
        //}
        //public IActionResult Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(Tag tag)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }
        //    if (_juanAppDbContext.Tags.Any(c => c.Name.Trim().ToLower() == tag.Name.Trim().ToLower()))
        //    {
        //        ModelState.AddModelError("Name", "This tag already exist");
        //        return View();
        //    }
        //    _juanAppDbContext.Tags.Add(tag);
        //    _juanAppDbContext.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    Tag? tag = _juanAppDbContext.Tags.Include(t => t.ProductsTags).FirstOrDefault(t => t.Id == id);
        //    if (tag == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(tag);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(Tag tag)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }
        //    if (_juanAppDbContext.Tags.Any(t => t.Name.Trim().ToLower() == tag.Name.Trim().ToLower() && t.Id != tag.Id))
        //    {
        //        ModelState.AddModelError("Name", "This genre already exist");
        //        return View();
        //    }
        //    Tag? existTag = _juanAppDbContext.Tags.Include(t => t.ProductsTags).FirstOrDefault(t => t.Id == tag.Id);
        //    if (tag == null)
        //    {
        //        return NotFound();
        //    }
        //    existTag.Name = tag.Name;
        //    _juanAppDbContext.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    Tag? tag = _juanAppDbContext.Tags.Include(t => t.ProductsTags).FirstOrDefault(t => t.Id == id);
        //    if (tag == null)
        //    {
        //        return NotFound();
        //    }
        //    _juanAppDbContext.Tags.Remove(tag);
        //    _juanAppDbContext.SaveChanges();

        //    return RedirectToAction("Index", "Tag");
        //}
        //public IActionResult Detail(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    Tag?     tag = _juanAppDbContext.Tags.Include(t => t.ProductsTags).FirstOrDefault(t => t.Id == id);
        //    if (tag == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(tag);
        //}
        #endregion


        #region with service
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        public IActionResult Index(int page = 1, int take = 2)
        {
            var paginatedList = _tagService.GetPaginatedTags(page, take);
            return View(paginatedList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!_tagService.CreateTag(tag, out string errorMessage))
            {
                ModelState.AddModelError("Name", errorMessage);
                return View();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = _tagService.GetTagById(id.Value);
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

            if (!_tagService.UpdateTag(tag, out string errorMessage))
            {
                ModelState.AddModelError("Name", errorMessage);
                return View();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || !_tagService.DeleteTag(id.Value))
            {
                return NotFound();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = _tagService.GetTagById(id.Value);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }
        #endregion
    }
}
