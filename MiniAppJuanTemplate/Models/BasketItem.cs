using MiniAppJuanTemplate.Models.Common;

namespace MiniAppJuanTemplate.Models
{
    public class BasketItem:BaseAuditableEntity
    {
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Count { get; set; }
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
    }
}
