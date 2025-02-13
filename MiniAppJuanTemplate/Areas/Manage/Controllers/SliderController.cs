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
    public class SliderController : Controller
    {
        private readonly JuanAppDbContext _juanAppDbContext;
        private readonly IWebHostEnvironment _env;

        public SliderController(JuanAppDbContext juanAppDbContext, IWebHostEnvironment env)
        {
            _juanAppDbContext = juanAppDbContext;
            _env = env;
        }

        public IActionResult Index()
        {
            return View(_juanAppDbContext.Sliders.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Slider slider)
        {
            if (slider.Photo == null)
            {
                ModelState.AddModelError("Photo", "Photo is required");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            var file = slider.Photo;
          
            if (_juanAppDbContext.Sliders.Any(s => s.Title.Trim().ToLower() == slider.Title.Trim().ToLower()))
            {
                ModelState.AddModelError("Title", "This slider already exist");
                return View();
            }
            slider.Image = file.SaveImage(_env.WebRootPath, "assets/img/slider");
           
           _juanAppDbContext.Sliders.Add(slider);
           _juanAppDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var slider =_juanAppDbContext.Sliders.Find(id);
            if (slider == null)
            {
                return BadRequest();
            }
            return View(slider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View(slider);
            }
            var existSlider = _juanAppDbContext.Sliders.Find(slider.Id);
            if (existSlider == null)
            {
                return BadRequest();
            }
            var file = slider.Photo;
            string oldImage = existSlider.Image;
            if (file != null)
            {
                existSlider.Image = file.SaveImage(_env.WebRootPath, "assets/image/bg-images");
                var deletedImagePath = Path.Combine(_env.WebRootPath, "assets/image/bg-images", oldImage);
                if (!FileManager.DeleteFile(deletedImagePath))
                {
                    return BadRequest();
                }
            }
            existSlider.Title = slider.Title;
            existSlider.Description = slider.Description;
            existSlider.Order = slider.Order;
            existSlider.ButtonLink = slider.ButtonLink;
            existSlider.ButtonText = slider.ButtonText;
            _juanAppDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int? id)
        {
            var existSlider = _juanAppDbContext.Sliders.Find(id);
            if (existSlider is null)
            {
                return BadRequest();
            }
            var deletedImagePath = Path.Combine(_env.WebRootPath, "assets/image/bg-images", existSlider.Image);
            if (!FileManager.DeleteFile(deletedImagePath))
            {
                return BadRequest();
            }
            _juanAppDbContext.Sliders.Remove(existSlider);
            _juanAppDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
           Slider slider = _juanAppDbContext.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider == null)
            {
                return NotFound();
            }
            return View(slider);
        }
    }
}
