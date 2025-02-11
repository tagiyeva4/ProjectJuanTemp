namespace MiniAppJuanTemplate.Models.Common
{
	public class BaseAuditableEntity:BaseEntity
	{
		public DateTime? CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public DateTime? DeletedDate { get; set; }
		public bool IsDeleted { get; set; } = false;
	}
}
