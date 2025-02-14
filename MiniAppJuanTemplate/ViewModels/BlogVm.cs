using MiniAppJuanTemplate.Models;

namespace MiniAppJuanTemplate.ViewModels
{
    public class BlogVm
    {
        public List<Tag> Tags { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<Blog> RecentBlogs { get; set; }
        public List<Category> Categories { get; set; }
    }
}
