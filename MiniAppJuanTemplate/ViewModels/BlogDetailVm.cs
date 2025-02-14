using MiniAppJuanTemplate.Models;

namespace MiniAppJuanTemplate.ViewModels
{
    public class BlogDetailVm
    {
        public Blog Blog { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Blog> RecentBlogs { get; set; }
        public List<Category> Categories { get; set; }

    }
}
