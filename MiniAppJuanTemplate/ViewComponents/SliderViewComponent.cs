using Microsoft.AspNetCore.Mvc;
using MiniAppJuanTemplate.Data;
using System.Threading.Tasks;

namespace MiniAppJuanTemplate.ViewComponents
{
    public class SliderViewComponent:ViewComponent
    {
        private readonly JuanAppDbContext _juanAppDbContext;

        public SliderViewComponent(JuanAppDbContext juanAppDbContext)
        {
            _juanAppDbContext = juanAppDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sliders = _juanAppDbContext.Sliders.ToList();
            return View(await Task.FromResult(sliders));
        }
    }
}
