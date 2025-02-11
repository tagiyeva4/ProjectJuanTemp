using MiniAppJuanTemplate.Models;

namespace MiniAppJuanTemplate.ViewModels
{
    public class HomeVm
    {
        public List<Slider> Sliders { get; set; }
        public List<HomeService> Services { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Blog> Blogs { get; set; }
    }
}
