using MiniAppJuanTemplate.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniAppJuanTemplate.Models
{
    public class Order:BaseAuditableEntity
    {
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public string? Country { get; set; }
        public string? CompanyName { get; set; }
        public string? TownCity { get; set; }
        public string? PhoneNumber { get; set; }
        public string? State { get; set; }
        public string? StreetAddress { get; set; }
        public string? ZipCode { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        public List<OrderItem>? OrderItems { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
    public enum OrderStatus
    {
        Pending,
        Accepted,
        Rejected,
        Cancelled,
    }
}
