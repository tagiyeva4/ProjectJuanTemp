using MiniAppJuanTemplate.Models.Common;

namespace MiniAppJuanTemplate.Models
{
    public class ProductImage:BaseEntity
    {
        public string Name { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
