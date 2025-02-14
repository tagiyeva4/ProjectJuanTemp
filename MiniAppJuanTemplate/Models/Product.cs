using MiniAppJuanTemplate.Attributes;
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
        public bool IsNew {  get; set; }
        public int Rate {  get; set; }
        public string MainImage { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<ProductTag> ProductTags { get; set; }
        public List<ProductSize> ProductSizes { get; set; }
        [NotMapped]
        public List<int> TagIds { get; set; }
        [NotMapped]
        public List<int> SizeIds { get; set; }
        [NotMapped]
        [MaxSize(2 * 1024 * 1024)]
        [AllowedTypeAttribute("image/jpeg", "image/png")]
        public IFormFile[]? Photos { get; set; }
        [NotMapped]
        [MaxSize(2 * 1024 * 1024)]
        [AllowedTypeAttribute("image/jpeg", "image/png")]
        public IFormFile? MainPhoto{ get; set; }

    }
}
