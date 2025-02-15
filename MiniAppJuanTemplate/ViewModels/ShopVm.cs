using MiniAppJuanTemplate.Models;

namespace MiniAppJuanTemplate.ViewModels
{
    public class ShopVm
    {
        public List<Category> Categories { get; set; }
        public List<ProductSize> ProductSizes { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Size> Sizes { get; set; }
        public List<Product> Products { get; set; }
    }
}
