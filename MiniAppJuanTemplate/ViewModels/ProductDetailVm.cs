using MiniAppJuanTemplate.Models;

namespace MiniAppJuanTemplate.ViewModels
{
    public class ProductDetailVm
    {
        public Product Product { get; set; }
        public List<Product> RelatedProducts { get; set; }
        public bool HasCommentUser { get; set; }
        public int TotalCommentsCount { get; set; }
        public int AvarageRate { get; set; }
        public ProductComment ProductComment { get; set; }
    }
}
