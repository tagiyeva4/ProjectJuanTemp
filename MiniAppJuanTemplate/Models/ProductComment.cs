using MiniAppJuanTemplate.Models.Common;

namespace MiniAppJuanTemplate.Models
{
    public class ProductComment : BaseAuditableEntity
    {
        public string Text { get; set; }
        public int Rate { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public CommentStatus Status { get; set; } = CommentStatus.Pending;
    }
    public enum CommentStatus
    {
        Pending,
        Accepted,
        Rejected
    }

}
