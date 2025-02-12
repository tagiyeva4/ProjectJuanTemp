using MiniAppJuanTemplate.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniAppJuanTemplate.Models
{
    public class Product:BaseAuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountPercentege { get; set; }
        public bool IsStock { get; set; }
        public bool NewProduct {  get; set; }
        public int Rate {  get; set; }
        public string MainImage { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<ProductTag> ProductTags { get; set; }
        public List<ProductSize> ProductSizes { get; set; }
    }
}
