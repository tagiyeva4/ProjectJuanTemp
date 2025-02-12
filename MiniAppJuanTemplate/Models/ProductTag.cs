using MiniAppJuanTemplate.Models.Common;

namespace MiniAppJuanTemplate.Models
{
    public class ProductTag:BaseEntity
    {
        public int ProductId { get; set; }
        public Product  Product { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }

    }
}
